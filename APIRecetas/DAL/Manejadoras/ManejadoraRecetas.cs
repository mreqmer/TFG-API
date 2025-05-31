using ClasesRecetas;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using DAL.Manejadoras;
using DAL.DTO.DTOReceta;

namespace DAL.Manejadoras
{
    /// <summary>
    /// Clase contenedora de lo métodos de conexión con la base de datos para la gestión de recetas.
    /// </summary>
    public class ManejadoraRecetas
    {
        #region Listados

        /// <summary>
        /// Obtiene un listado de todas las recetas con sus respectivos creadores.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<RecetaUsuario> ObtieneListadoRecetas()
        {
            List<RecetaUsuario> listadoRecetas = new List<RecetaUsuario>();

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    string query = @"
                        SELECT r.*, u.NombreUsuario 
                        FROM Recetas r 
                        INNER JOIN Usuarios u ON r.IdCreador = u.IdUsuario";

                    using (SqlCommand miComando = new SqlCommand(query, miConexion))
                    using (SqlDataReader miLector = miComando.ExecuteReader())
                    {
                        while (miLector.Read())
                        {
                            RecetaUsuario receta = new RecetaUsuario
                            {
                                IdReceta = (int)miLector["IdReceta"],
                                NombreReceta = (string)miLector["NombreReceta"],
                                Descripcion = (string)miLector["Descripcion"],
                                TiempoPreparacion = (int)miLector["TiempoPreparacion"],
                                Dificultad = (string)miLector["Dificultad"],
                                FechaCreacion = (DateTime)miLector["FechaCreacion"],
                                FotoReceta = (string)miLector["FotoReceta"],
                                NombreUsuario = (string)miLector["NombreUsuario"],
                                IdCreador = (int)miLector["IdCreador"]
                            };

                            listadoRecetas.Add(receta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las recetas", ex);
            }

            return listadoRecetas;
        }

        /// <summary>
        /// Obtiene un listado de recetas con el nombre del creador y si tiene like
        /// </summary>
        /// <param name="firebaseUID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<RecetaUsuarioLike> ObtieneListadoRecetasConLike(string firebaseUID)
        {
            List<RecetaUsuarioLike> listadoRecetas = new List<RecetaUsuarioLike>();

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    string query = @"
                                SELECT r.*, u.NombreUsuario,
                                       CASE WHEN EXISTS (
                                           SELECT 1 FROM Likes l2
                                           INNER JOIN Usuarios ul2 ON l2.IdUsuario = ul2.IdUsuario
                                           WHERE l2.IdReceta = r.IdReceta AND ul2.FirebaseUID = @firebaseUID
                                       ) THEN 1 ELSE 0 END AS TieneLike
                                FROM Recetas r
                                INNER JOIN Usuarios u ON r.IdCreador = u.IdUsuario";

                    using (SqlCommand miComando = new SqlCommand(query, miConexion))
                    {
                        miComando.Parameters.AddWithValue("@firebaseUID", firebaseUID);

                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            while (miLector.Read())
                            {
                                RecetaUsuarioLike receta = new RecetaUsuarioLike
                                {
                                    IdReceta = (int)miLector["IdReceta"],
                                    NombreReceta = (string)miLector["NombreReceta"],
                                    Descripcion = (string)miLector["Descripcion"],
                                    TiempoPreparacion = (int)miLector["TiempoPreparacion"],
                                    Dificultad = (string)miLector["Dificultad"],
                                    FechaCreacion = (DateTime)miLector["FechaCreacion"],
                                    FotoReceta = (string)miLector["FotoReceta"],
                                    NombreUsuario = (string)miLector["NombreUsuario"],
                                    IdCreador = (int)miLector["IdCreador"],
                                    TieneLike = (int)miLector["TieneLike"] == 1
                                };

                                listadoRecetas.Add(receta);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las recetas con like", ex);
            }

            return listadoRecetas;
        }

        /// <summary>
        /// Obtiene un listado de recetas con el nombre del creador y si tiene like, filtrado por parámetros opcionales.
        /// </summary>
        /// <param name="firebaseUID"></param>
        /// <param name="busqueda"></param>
        /// <param name="categoria"></param>
        /// <param name="tiempoMaxMinutos"></param>
        /// <param name="dificultad"></param>
        /// <param name="ingredientes"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<RecetaUsuarioLike> ObtieneRecetasConLikeFiltrado(
               string firebaseUID,
               string busqueda,
               string? categoria,
               int? tiempoMaxMinutos,
               string? dificultad,
               List<string>? ingredientes)
        {
            List<RecetaUsuarioLike> listadoRecetas = new List<RecetaUsuarioLike>();

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();

                    string query = @"
                        SELECT r.*, u.NombreUsuario,
                               CASE WHEN EXISTS (
                                   SELECT 1 FROM Likes l2
                                   INNER JOIN Usuarios ul2 ON l2.IdUsuario = ul2.IdUsuario
                                   WHERE l2.IdReceta = r.IdReceta AND ul2.FirebaseUID = @firebaseUID
                               ) THEN 1 ELSE 0 END AS TieneLike
                        FROM Recetas r
                        INNER JOIN Usuarios u ON r.IdCreador = u.IdUsuario
                        WHERE LOWER(r.NombreReceta) LIKE '%' + LOWER(@busqueda) + '%'
                          AND (@categoria IS NULL OR EXISTS (
                              SELECT 1 FROM RecetaCategoria rc
                              INNER JOIN Categoria c ON rc.IdCategoria = c.IdCategoria
                              WHERE rc.IdReceta = r.IdReceta AND LOWER(c.Nombre) = LOWER(@categoria)
                          ))
                          AND (@tiempoMax IS NULL OR r.TiempoPreparacion <= @tiempoMax)
                          AND (@dificultad IS NULL OR LOWER(r.Dificultad) = LOWER(@dificultad))";

                    if (ingredientes != null && ingredientes.Count > 0)
                    {
                        string ingredientesUnion = string.Join(" UNION ALL ", ingredientes.Select((i, index) => $"SELECT @ingrediente{index} AS nombre"));
                        query += $@"
                          AND NOT EXISTS (
                              SELECT nombre FROM (
                                  {ingredientesUnion}
                              ) AS filtro
                              EXCEPT
                              SELECT i2.NombreIngrediente
                              FROM Ingredientes i2
                              INNER JOIN Receta_Ingredientes ri2 ON i2.IdIngrediente = ri2.IdIngrediente
                              WHERE ri2.IdReceta = r.IdReceta
                          )";
                                            }

                    query += " ORDER BY r.FechaCreacion DESC;";

                    using (SqlCommand miComando = new SqlCommand(query, miConexion))
                    {
                        miComando.Parameters.AddWithValue("@firebaseUID", firebaseUID);
                        miComando.Parameters.AddWithValue("@busqueda", busqueda ?? string.Empty);
                        miComando.Parameters.AddWithValue("@categoria", (object?)categoria ?? DBNull.Value);
                        miComando.Parameters.AddWithValue("@tiempoMax", (object?)tiempoMaxMinutos ?? DBNull.Value);
                        miComando.Parameters.AddWithValue("@dificultad", (object?)dificultad ?? DBNull.Value);

                        if (ingredientes != null && ingredientes.Count > 0)
                        {
                            for (int i = 0; i < ingredientes.Count; i++)
                            {
                                miComando.Parameters.AddWithValue($"@ingrediente{i}", ingredientes[i]);
                            }
                        }

                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            while (miLector.Read())
                            {
                                RecetaUsuarioLike receta = new RecetaUsuarioLike
                                {
                                    IdReceta = (int)miLector["IdReceta"],
                                    NombreReceta = (string)miLector["NombreReceta"],
                                    Descripcion = (string)miLector["Descripcion"],
                                    TiempoPreparacion = (int)miLector["TiempoPreparacion"],
                                    Dificultad = (string)miLector["Dificultad"],
                                    FechaCreacion = (DateTime)miLector["FechaCreacion"],
                                    FotoReceta = (string)miLector["FotoReceta"],
                                    NombreUsuario = (string)miLector["NombreUsuario"],
                                    IdCreador = (int)miLector["IdCreador"],
                                    TieneLike = (int)miLector["TieneLike"] == 1
                                };

                                listadoRecetas.Add(receta);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las recetas filtradas", ex);
            }

            return listadoRecetas;
        }

        /// <summary>
        /// Obtiene un listado de recetas creadas por un usuario específico, dado su IdCreador.
        /// </summary>
        /// <param name="idCreador"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<RecetaUsuario> ObtieneRecetasPorIdCreador(int idCreador)
        {
            List<RecetaUsuario> listadoRecetas = new List<RecetaUsuario>();

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    string query = @"
                        SELECT r.*, u.NombreUsuario 
                        FROM Recetas r 
                        INNER JOIN Usuarios u ON r.IdCreador = u.IdUsuario 
                        WHERE r.IdCreador = @idCreador";

                    using (SqlCommand miComando = new SqlCommand(query, miConexion))
                    {
                        miComando.Parameters.AddWithValue("@idCreador", idCreador);

                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            while (miLector.Read())
                            {
                                RecetaUsuario receta = new RecetaUsuario
                                {
                                    IdReceta = (int)miLector["IdReceta"],
                                    NombreReceta = (string)miLector["NombreReceta"],
                                    Descripcion = (string)miLector["Descripcion"],
                                    TiempoPreparacion = (int)miLector["TiempoPreparacion"],
                                    Dificultad = (string)miLector["Dificultad"],
                                    FechaCreacion = (DateTime)miLector["FechaCreacion"],
                                    FotoReceta = (string)miLector["FotoReceta"],
                                    NombreUsuario = (string)miLector["NombreUsuario"],
                                    IdCreador = (int)miLector["IdCreador"]
                                };

                                listadoRecetas.Add(receta);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las recetas por IdCreador", ex);
            }

            return listadoRecetas;
        }

        /// <summary>
        /// Obtiene un listado de recetas favoritas de un usuario, dado su Firebase UID.
        /// </summary>
        /// <param name="firebaseUid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<RecetaUsuarioLike> ObtieneRecetasFavoritasPorUid(string firebaseUid)
        {
            List<RecetaUsuarioLike> listadoRecetas = new List<RecetaUsuarioLike>();

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();

                    string query = @"
                SELECT r.*, u.NombreUsuario
                FROM Recetas r
                INNER JOIN Usuarios u ON r.IdCreador = u.IdUsuario
                INNER JOIN Likes l ON l.IdReceta = r.IdReceta
                INNER JOIN Usuarios ul ON l.IdUsuario = ul.IdUsuario
                WHERE ul.FirebaseUID = @firebaseUid";

                    using (SqlCommand miComando = new SqlCommand(query, miConexion))
                    {
                        miComando.Parameters.AddWithValue("@firebaseUid", firebaseUid);

                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            while (miLector.Read())
                            {
                                RecetaUsuarioLike receta = new RecetaUsuarioLike
                                {
                                    IdReceta = (int)miLector["IdReceta"],
                                    NombreReceta = (string)miLector["NombreReceta"],
                                    Descripcion = (string)miLector["Descripcion"],
                                    TiempoPreparacion = (int)miLector["TiempoPreparacion"],
                                    Dificultad = (string)miLector["Dificultad"],
                                    FechaCreacion = (DateTime)miLector["FechaCreacion"],
                                    FotoReceta = (string)miLector["FotoReceta"],
                                    NombreUsuario = (string)miLector["NombreUsuario"],
                                    IdCreador = (int)miLector["IdCreador"],
                                    TieneLike = true
                                };

                                listadoRecetas.Add(receta);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las recetas favoritas del usuario por UID", ex);
            }

            return listadoRecetas;
        }
        #endregion

        #region Objetos
        /// <summary>
        /// Obtiene una receta por su ID, incluyendo sus pasos, ingredientes y categorías.
        /// </summary>
        /// <param name="idReceta"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static RecetaDTO ObtieneRecetaID(int idReceta)
        {
            RecetaDTO oReceta = new RecetaDTO();
            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    string query = @"
                        SELECT r.*, u.NombreUsuario 
                        FROM Recetas r 
                        INNER JOIN Usuarios u ON r.IdCreador = u.IdUsuario 
                        WHERE r.IdReceta = @idReceta";

                    using (SqlCommand miComando = new SqlCommand(query, miConexion))
                    {
                        miComando.Parameters.AddWithValue("@idReceta", idReceta);

                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            if (miLector.HasRows)
                            {
                                while (miLector.Read())
                                {
                                    oReceta.IdReceta = (int)miLector["IdReceta"];
                                    oReceta.NombreReceta = (string)miLector["NombreReceta"];
                                    oReceta.Descripcion = (string)miLector["Descripcion"];
                                    oReceta.TiempoPreparacion = (int)miLector["TiempoPreparacion"];
                                    oReceta.Dificultad = (string)miLector["Dificultad"];
                                    oReceta.FechaCreacion = (DateTime)miLector["FechaCreacion"];
                                    oReceta.FotoReceta = (string)miLector["FotoReceta"];
                                    oReceta.NombreUsuario = (string)miLector["NombreUsuario"];
                                }
                            }
                        }
                    }

                    oReceta.Pasos = ManejadoraPasos.ObtienePasosReceta(idReceta);
                    oReceta.Ingredientes = ManejadoraIngredientes.ObtieneIngredientesReceta(idReceta);
                    oReceta.Categorias = ManejadoraCategorias.ObtieneCategoriasReceta(idReceta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la receta", ex);
            }
            return oReceta;
        }

        /// <summary>
        /// Obtiene los detalles de una receta, con la información del like
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DTORecetaDetalladaLike ObtieneRecetaIDFavorito(DTOGetRecetaDetalladaLike model)
        {
            DTORecetaDetalladaLike oReceta = new DTORecetaDetalladaLike();
            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    string query = @"
                        SELECT  r.*,  u.NombreUsuario,
                            CASE 
                                WHEN l.IdLike IS NOT NULL THEN 1 
                                ELSE 0 
                            END AS TieneLike
                        FROM Recetas r 
                        INNER JOIN Usuarios u ON r.IdCreador = u.IdUsuario 
                        LEFT JOIN Likes l ON l.IdReceta = r.IdReceta 
                        AND l.IdUsuario = (SELECT IdUsuario FROM Usuarios WHERE FirebaseUID = @firebaseUID)
                        WHERE r.IdReceta = @idReceta";

                    using (SqlCommand miComando = new SqlCommand(query, miConexion))
                    {
                        miComando.Parameters.AddWithValue("@idReceta", model.IdReceta);
                        miComando.Parameters.AddWithValue("@firebaseUID", model.Uid);

                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            if (miLector.HasRows)
                            {
                                while (miLector.Read())
                                {
                                    oReceta.IdReceta = (int)miLector["IdReceta"];
                                    oReceta.NombreReceta = (string)miLector["NombreReceta"];
                                    oReceta.Descripcion = (string)miLector["Descripcion"];
                                    oReceta.TiempoPreparacion = (int)miLector["TiempoPreparacion"];
                                    oReceta.Dificultad = (string)miLector["Dificultad"];
                                    oReceta.FechaCreacion = (DateTime)miLector["FechaCreacion"];
                                    oReceta.FotoReceta = (string)miLector["FotoReceta"];
                                    oReceta.NombreUsuario = (string)miLector["NombreUsuario"];
                                    oReceta.TieneLike = (int)miLector["TieneLike"] == 1;
                                }
                            }
                        }
                    }

                    oReceta.Pasos = ManejadoraPasos.ObtienePasosReceta(model.IdReceta);
                    oReceta.Ingredientes = ManejadoraIngredientes.ObtieneIngredientesReceta(model.IdReceta);
                    oReceta.Categorias = ManejadoraCategorias.ObtieneCategoriasReceta(model.IdReceta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la receta", ex);
            }
            return oReceta;
        }

        /// <summary>
        /// Obtiene una receta por su nombre, incluyendo sus pasos, ingredientes y categorías.
        /// </summary>
        /// <param name="nombreReceta"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static RecetaDTO ObtieneRecetaNombre(string nombreReceta)
        {
            RecetaDTO oReceta = new RecetaDTO();
            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    string query = @"
                        SELECT r.*, u.NombreUsuario 
                        FROM Recetas r 
                        INNER JOIN Usuarios u ON r.IdCreador = u.IdUsuario 
                        WHERE r.NombreReceta = @nombreReceta";

                    using (SqlCommand miComando = new SqlCommand(query, miConexion))
                    {
                        miComando.Parameters.AddWithValue("@nombreReceta", nombreReceta);

                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            if (miLector.HasRows)
                            {
                                while (miLector.Read())
                                {
                                    oReceta.IdReceta = (int)miLector["IdReceta"];
                                    oReceta.NombreReceta = (string)miLector["NombreReceta"];
                                    oReceta.Descripcion = (string)miLector["Descripcion"];
                                    oReceta.TiempoPreparacion = (int)miLector["TiempoPreparacion"];
                                    oReceta.Dificultad = (string)miLector["Dificultad"];
                                    oReceta.FechaCreacion = (DateTime)miLector["FechaCreacion"];
                                    oReceta.FotoReceta = (string)miLector["FotoReceta"];
                                    oReceta.NombreUsuario = (string)miLector["NombreUsuario"];
                                }
                            }
                        }
                    }

                    oReceta.Pasos = ManejadoraPasos.ObtienePasosReceta(oReceta.IdReceta);
                    oReceta.Ingredientes = ManejadoraIngredientes.ObtieneIngredientesReceta(oReceta.IdReceta);
                    oReceta.Categorias = ManejadoraCategorias.ObtieneCategoriasReceta(oReceta.IdReceta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la receta por nombre", ex);
            }
            return oReceta;
        }

        #endregion

        #region insert
        /// <summary>
        /// Crea una nueva receta completa, incluyendo pasos, ingredientes y categorías.
        /// </summary>
        /// <param name="receta"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DTONuevaReceta InsertaRecetaCompleta(DTONuevaReceta receta)
        {
            using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
            {
                miConexion.Open();
                SqlTransaction transaccion = miConexion.BeginTransaction();

                try
                {
                    // Insertar Receta
                    int idReceta;
                    using (SqlCommand miComando = new SqlCommand(@"
                    INSERT INTO Recetas (NombreReceta, Descripcion, TiempoPreparacion, Dificultad, FechaCreacion, FotoReceta, IdCreador)
                    VALUES (@nombreReceta, @descripcion, @tiempoPreparacion, @dificultad, @fechaCreacion, @fotoReceta, @idCreador);
                    SELECT SCOPE_IDENTITY();", miConexion, transaccion))
                    {
                        miComando.Parameters.AddWithValue("@nombreReceta", receta.NombreReceta);
                        miComando.Parameters.AddWithValue("@descripcion", receta.Descripcion);
                        miComando.Parameters.AddWithValue("@tiempoPreparacion", receta.TiempoPreparacion);
                        miComando.Parameters.AddWithValue("@dificultad", receta.Dificultad);
                        miComando.Parameters.AddWithValue("@fechaCreacion", receta.FechaCreacion);
                        miComando.Parameters.AddWithValue("@fotoReceta", receta.FotoReceta);
                        miComando.Parameters.AddWithValue("@idCreador", receta.IdCreador); // Aquí se usa directamente IdCreador

                        idReceta = Convert.ToInt32(miComando.ExecuteScalar());
                    }

                    receta.IdReceta = idReceta;

                    // Insertar Pasos
                    foreach (var paso in receta.Pasos)
                    {
                        using (SqlCommand miComando = new SqlCommand(@"
                            INSERT INTO PasosReceta (IdReceta, Orden, Descripcion)
                            VALUES (@idReceta, @orden, @descripcion);", miConexion, transaccion))
                        {
                            miComando.Parameters.AddWithValue("@idReceta", idReceta);
                            miComando.Parameters.AddWithValue("@orden", paso.Orden);
                            miComando.Parameters.AddWithValue("@descripcion", paso.Descripcion);

                            miComando.ExecuteNonQuery();
                        }
                    }

                    // Insertar Ingredientes
                    foreach (var ingrediente in receta.Ingredientes)
                    {
                        using (SqlCommand miComando = new SqlCommand(@"
                            INSERT INTO Receta_Ingredientes (IdReceta, IdIngrediente, Cantidad, Notas)
                            VALUES (@idReceta, @idIngrediente, @cantidad, @notas);", miConexion, transaccion))
                        {
                            miComando.Parameters.AddWithValue("@idReceta", idReceta);
                            miComando.Parameters.AddWithValue("@idIngrediente", ingrediente.IdIngrediente);
                            miComando.Parameters.AddWithValue("@cantidad", ingrediente.Cantidad);
                            miComando.Parameters.AddWithValue("@notas", ingrediente.Notas);

                            miComando.ExecuteNonQuery();
                        }
                    }

                    // Insertar Categorías
                    foreach (var categoria in receta.Categorias)
                    {
                        using (SqlCommand miComando = new SqlCommand(@"
                            INSERT INTO RecetaCategoria (IdReceta, IdCategoria)
                            VALUES (@idReceta, @idCategoria);", miConexion, transaccion))
                        {
                            miComando.Parameters.AddWithValue("@idReceta", idReceta);
                            miComando.Parameters.AddWithValue("@idCategoria", categoria.IdCategoria);

                            miComando.ExecuteNonQuery();
                        }
                    }

                    transaccion.Commit();

                    // Aquí se devuelve el DTO con los datos completos
                    return receta;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw new Exception("Error al insertar la receta completa", ex);
                }
            }
        }
        #endregion

        #region Borrado
        /// <summary>
        /// Borra una receta por su ID y verifica que el usuario que la borra es el creador de la misma.
        /// </summary>
        /// <param name="firebaseUID"></param>
        /// <param name="idReceta"></param>
        /// <exception cref="Exception"></exception>
        public static void BorrarRecetaPorIdYUid(string firebaseUID, int idReceta)
        {
            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();

                    // 1. Verificación de propiedad
                    string queryVerificacion = @"
                SELECT r.IdReceta
                FROM Recetas r
                INNER JOIN Usuarios u ON r.IdCreador = u.IdUsuario
                WHERE r.IdReceta = @idReceta AND u.FirebaseUID = @firebaseUID";

                    using (SqlCommand cmdVerificar = new SqlCommand(queryVerificacion, miConexion))
                    {
                        cmdVerificar.Parameters.AddWithValue("@idReceta", idReceta);
                        cmdVerificar.Parameters.AddWithValue("@firebaseUID", firebaseUID);

                        object resultado = cmdVerificar.ExecuteScalar();

                        if (resultado == null)
                        {
                            throw new Exception($"No se encontró la receta con Id {idReceta} perteneciente al usuario con UID '{firebaseUID}'.");
                        }
                    }

                    // 2. Eliminación en orden con transacción
                    using (SqlTransaction transaccion = miConexion.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = miConexion.CreateCommand())
                            {
                                cmd.Transaction = transaccion;
                                cmd.Parameters.AddWithValue("@idReceta", idReceta);

                                cmd.CommandText = "DELETE FROM Likes WHERE IdReceta = @idReceta";
                                int likesEliminados = cmd.ExecuteNonQuery();

                                cmd.CommandText = "DELETE FROM PasosReceta WHERE IdReceta = @idReceta";
                                int pasosEliminados = cmd.ExecuteNonQuery();

                                cmd.CommandText = "DELETE FROM Receta_Ingredientes WHERE IdReceta = @idReceta";
                                int ingredientesEliminados = cmd.ExecuteNonQuery();

                                cmd.CommandText = "DELETE FROM RecetaCategoria WHERE IdReceta = @idReceta";
                                int categoriasEliminadas = cmd.ExecuteNonQuery();

                                cmd.CommandText = "DELETE FROM Recetas WHERE IdReceta = @idReceta";
                                int recetasEliminadas = cmd.ExecuteNonQuery();

                                if (recetasEliminadas == 0)
                                {
                                    throw new Exception("No se pudo eliminar la receta principal. Puede que ya no exista.");
                                }

                                transaccion.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw new Exception("Error durante la transacción de borrado: " + e.Message, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al borrar la receta: " + ex.Message, ex);
            }
        }
        #endregion
 
    }
}

using ClasesRecetas;
using DAL.DTO;
using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Manejadoras
{
    public class ManejadoraRecetas
    {
        #region Listados

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

        public static List<RecetaUsuarioLike> ObtieneRecetasConLikePorNombre(string firebaseUID, string busqueda)
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
                        WHERE LOWER(r.NombreReceta) LIKE '%' + LOWER(@busqueda) + '%'";

                    using (SqlCommand miComando = new SqlCommand(query, miConexion))
                    {
                        miComando.Parameters.AddWithValue("@firebaseUID", firebaseUID);
                        miComando.Parameters.AddWithValue("@busqueda", busqueda);

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
                throw new Exception("Error al obtener las recetas con like por nombre", ex);
            }

            return listadoRecetas;
        }
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

                    oReceta.Pasos = ObtienePasosReceta(idReceta);
                    oReceta.Ingredientes = ObtieneIngredientesReceta(idReceta);
                    oReceta.Categorias = ObtieneCategoriasReceta(idReceta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la receta", ex);
            }
            return oReceta;
        }

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

                    oReceta.Pasos = ObtienePasosReceta(model.IdReceta);
                    oReceta.Ingredientes = ObtieneIngredientesReceta(model.IdReceta);
                    oReceta.Categorias = ObtieneCategoriasReceta(model.IdReceta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la receta", ex);
            }
            return oReceta;
        }

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

                    oReceta.Pasos = ObtienePasosReceta(oReceta.IdReceta);
                    oReceta.Ingredientes = ObtieneIngredientesReceta(oReceta.IdReceta);
                    oReceta.Categorias = ObtieneCategoriasReceta(oReceta.IdReceta);
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

        #region Métodos Auxiliares

        public static List<PasoRecetaDTO> ObtienePasosReceta(int idReceta)
        {
            List<PasoRecetaDTO> pasoRecetas = new List<PasoRecetaDTO>();
            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    using (SqlCommand miComando = new SqlCommand("ObtenerPasosReceta", miConexion))
                    {
                        miComando.CommandType = CommandType.StoredProcedure;
                        miComando.Parameters.AddWithValue("@p_idReceta", idReceta);

                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            while (miLector.Read())
                            {
                                PasoRecetaDTO oPaso = new PasoRecetaDTO
                                {
                                    Orden = (short)miLector["Orden"],
                                    Descripcion = (string)miLector["Descripcion"]
                                };
                                pasoRecetas.Add(oPaso);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pasos de la receta", ex);
            }
            return pasoRecetas;
        }

        public static List<IngredienteRecetaDTO> ObtieneIngredientesReceta(int idReceta)
        {
            List<IngredienteRecetaDTO> ingredientesReceta = new List<IngredienteRecetaDTO>();
            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    using (SqlCommand miComando = new SqlCommand("ObtenerIngredientesReceta", miConexion))
                    {
                        miComando.CommandType = CommandType.StoredProcedure;
                        miComando.Parameters.AddWithValue("@p_idReceta", idReceta);
                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            while (miLector.Read())
                            {
                                IngredienteRecetaDTO oIngrediente = new IngredienteRecetaDTO
                                {
                                    IdIngrediente = (int)miLector["idIngrediente"],
                                    NombreIngrediente = (string)miLector["NombreIngrediente"],
                                    Categoria = (string)miLector["Categoria"],
                                    Medida = (string)miLector["Medida"],
                                    Cantidad = (decimal)miLector["Cantidad"],
                                    Notas = miLector["Notas"] as string ?? string.Empty,
                                };
                                ingredientesReceta.Add(oIngrediente);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los ingredientes de la receta", ex);
            }
            return ingredientesReceta;
        }

        public static List<CategoriaRecetaDTO> ObtieneCategoriasReceta(int idReceta)
        {
            List<CategoriaRecetaDTO> categoriaRecetas = new List<CategoriaRecetaDTO>();
            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    using (SqlCommand miComando = new SqlCommand("ObtenerCategoriasReceta", miConexion))
                    {
                        miComando.CommandType = CommandType.StoredProcedure;
                        miComando.Parameters.AddWithValue("@p_idReceta", idReceta);
                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            while (miLector.Read())
                            {
                                CategoriaRecetaDTO oCategoria = new CategoriaRecetaDTO
                                {
                                    IdCategoria = (int)miLector["idCategoria"],
                                    NombreCategoria = (string)miLector["nombre"]
                                };
                                categoriaRecetas.Add(oCategoria);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las categorías de la receta", ex);
            }
            return categoriaRecetas;
        }



        #endregion
    }
}

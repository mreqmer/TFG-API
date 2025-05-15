using ClasesRecetas;
using DAL.DTO;
using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
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

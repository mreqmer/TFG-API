using ClasesRecetas;
using DAL.DTO;
using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ManejadoraRecetas
    {
        #region Listados
        public static List<Receta> ObtieneListadoRecetas()
        {
            List<Receta> listadoRecetas = new List<Receta>();
            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    using (SqlCommand miComando = new SqlCommand("SELECT * FROM Recetas", miConexion))
                    using (SqlDataReader miLector = miComando.ExecuteReader())
                    {
                        while (miLector.Read())
                        {
                            Receta oReceta = new Receta
                            {
                                IdReceta = (int)miLector["IdReceta"],
                                NombreReceta = (string)miLector["NombreReceta"],
                                Descripcion = (string)miLector["Descripcion"],
                                TiempoPreparacion = (int)miLector["TiempoPreparacion"],
                                Dificultad = (string)miLector["Dificultad"],
                                FechaCreacion = (DateTime)miLector["FechaCreacion"]
                            };
                            listadoRecetas.Add(oReceta);
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
                    using (SqlCommand miComando = new SqlCommand("SELECT * FROM Recetas WHERE idReceta = @idReceta", miConexion))
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
                                }
                            }
                        }
                    }

                    // Obtener pasos
                    oReceta.Pasos = ObtienePasosReceta(idReceta);

                    // Obtener ingredientes
                    oReceta.Ingredientes = ObtieneIngredientesReceta(idReceta);

                    // Obtener categorías
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
                    using (SqlCommand miComando = new SqlCommand("SELECT * FROM Recetas WHERE NombreReceta = @nombreReceta", miConexion))
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
                                }
                            }
                        }
                    }

                    // Obtener pasos
                    oReceta.Pasos = ObtienePasosReceta(oReceta.IdReceta);

                    // Obtener ingredientes
                    oReceta.Ingredientes = ObtieneIngredientesReceta(oReceta.IdReceta);

                    // Obtener categorías
                    oReceta.Categorias = ObtieneCategoriasReceta(oReceta.IdReceta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la receta", ex);
            }
            return oReceta;
        }


        #endregion

    }
}

using ClasesRecetas;
using DAL.DTO.DTOCategoria;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manejadoras
{
    public class ManejadoraCategorias
    {
        /// <summary>
        /// Obtiene un listado de todas las categorías disponibles en la base de datos.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<Categoria> ObtieneListadoCategorias()
        {
            SqlConnection miConexion = new SqlConnection();
            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;
            miConexion.ConnectionString = Conexion.CadenaConexion();
            List<Categoria> listadoCategorias = new List<Categoria>();

            try
            {
                miConexion.Open();
                miComando.CommandText = "SELECT IdCategoria, Nombre FROM Categoria";
                miComando.Connection = miConexion;
                miLector = miComando.ExecuteReader();
                Categoria oCategoria;

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        oCategoria = new Categoria();
                        oCategoria.IdCategoria = (int)miLector["IdCategoria"];
                        oCategoria.NombreCategoria = (string)miLector["Nombre"];
                        listadoCategorias.Add(oCategoria);
                    }
                }
                miLector.Close();
                miConexion.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el listado de categorías", ex);
            }

            return listadoCategorias;
        }

        /// <summary>
        /// Obtiene las categorías asociadas a una receta específica mediante su ID.
        /// </summary>
        /// <param name="idReceta"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

    }
}

using ClasesRecetas;
using DAL.DTO.DTOIngrediente;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manejadoras
{
    public class ManejadoraIngredientes
    {
        #region listados
        /// <summary>
        /// Obtiene un listado de todos los ingredientes de la base de datos.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<Ingrediente> ObtieneListadoIngredientes()
        {
            SqlConnection miConexion = new SqlConnection();
            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;
            miConexion.ConnectionString = Conexion.CadenaConexion();
            List<Ingrediente> listadoIngredientes = new List<Ingrediente>();
            try
            {
                miConexion.Open();
                miComando.CommandText = "SELECT * FROM Ingredientes";
                miComando.Connection = miConexion;
                miLector = miComando.ExecuteReader();
                Ingrediente oIngrediente;
                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        oIngrediente = new Ingrediente();
                        oIngrediente.IdIngrediente = (int)miLector["IdIngrediente"];
                        oIngrediente.NombreIngrediente = (string)miLector["NombreIngrediente"];
                        oIngrediente.Categoria = (string)miLector["Categoria"];
                        oIngrediente.Medida = (string)miLector["Medida"];
                        listadoIngredientes.Add(oIngrediente);
                    }
                }
                miLector.Close();
                miConexion.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el listado de ingredientes", ex);
            }


            return listadoIngredientes;
        }

        /// <summary>
        /// Obtiene un listado de ingredientes que coinciden con la búsqueda por nombre.
        /// </summary>
        /// <param name="busquedaNombre"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<Ingrediente> ObtieneIngredienteBuscador(string busquedaNombre)
        {
            SqlConnection miConexion = new SqlConnection();
            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;
            miConexion.ConnectionString = Conexion.CadenaConexion();
            List<Ingrediente> listadoIngredientes = new List<Ingrediente>();

            try
            {
                miConexion.Open();

                miComando.CommandText = "SELECT * FROM Ingredientes WHERE NombreIngrediente LIKE @BusquedaNombre + '%'";
                miComando.Parameters.AddWithValue("@BusquedaNombre", busquedaNombre);
                miComando.Connection = miConexion;

                miLector = miComando.ExecuteReader();

                Ingrediente oIngrediente;
                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        oIngrediente = new Ingrediente();
                        oIngrediente.IdIngrediente = (int)miLector["IdIngrediente"];
                        oIngrediente.NombreIngrediente = (string)miLector["NombreIngrediente"];
                        oIngrediente.Categoria = (string)miLector["Categoria"];
                        oIngrediente.Medida = (string)miLector["Medida"];
                        listadoIngredientes.Add(oIngrediente);
                    }
                }

                miLector.Close();
                miConexion.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los ingredientes por búsqueda", ex);
            }

            return listadoIngredientes;
        }

        /// <summary>
        /// Obtiene los ingredientes de una receta específica por su ID.
        /// </summary>
        /// <param name="idReceta"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<DTOIngredienteReceta> ObtieneIngredientesReceta(int idReceta)
        {
            List<DTOIngredienteReceta> ingredientesReceta = new List<DTOIngredienteReceta>();
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
                                DTOIngredienteReceta oIngrediente = new DTOIngredienteReceta
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

        #region objetos
        /// <summary>
        /// Obtiene un ingrediente específico por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Ingrediente ObtieneIngrediente(int id)
        {
            SqlConnection miConexion = new SqlConnection();
            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;
            miConexion.ConnectionString = Conexion.CadenaConexion();
            Ingrediente oIngrediente = new Ingrediente();
            try
            {
                miConexion.Open();
                miComando.Parameters.AddWithValue("@id", id);
                miComando.CommandText = "SELECT * FROM Ingredientes WHERE idIngrediente = @id";
                miComando.Connection = miConexion;
                miLector = miComando.ExecuteReader();
                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        oIngrediente = new Ingrediente();
                        oIngrediente.IdIngrediente = (int)miLector["IdIngrediente"];
                        oIngrediente.NombreIngrediente = (string)miLector["NombreIngrediente"];
                        oIngrediente.Categoria = (string)miLector["Categoria"];
                        oIngrediente.Medida = (string)miLector["Medida"];
                    }
                }
                miLector.Close();
                miConexion.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ingrediente", ex);
            }


            return oIngrediente;
        }


        #endregion

       


    }
}

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

        public static List<Ingrediente> ObtieneListadoIngredientesCategorias(string categoria)
        {
            SqlConnection miConexion = new SqlConnection();
            List<Ingrediente> listadoIngredientesCategorias = new List<Ingrediente>();
            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;
            Ingrediente oIngrediente;
            miConexion.ConnectionString = Conexion.CadenaConexion();
            try
            {
                miConexion.Open();
                miComando.Parameters.AddWithValue("@categoria", categoria);
                miComando.CommandText = "SELECT * FROM Ingredientes WHERE Categoria = @categoria";
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
                        listadoIngredientesCategorias.Add(oIngrediente);
                    }

                }
                miLector.Close();
                miConexion.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el listado de ingredientes", ex);
            }
            return listadoIngredientesCategorias;
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

        #region objetos
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

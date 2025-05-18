using ClasesRecetas;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manejadoras
{
    public class ManejadoraCategorias
    {

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

    }
}

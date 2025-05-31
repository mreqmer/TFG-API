using DAL.DTO.DTOPasos;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manejadoras
{
    public class ManejadoraPasos
    {
        /// <summary>
        /// Obtiene los pasos de una receta a partir de su ID.
        /// </summary>
        /// <param name="idReceta"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

    }
}

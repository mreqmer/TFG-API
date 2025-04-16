using ClasesRecetas;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ManejadoraUsuarios
    {
        public static Usuario ObtieneUsuarioPorUID(string firebaseUID)
        {
            Usuario usuario = null;

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();
                    using (SqlCommand miComando = new SqlCommand("SELECT * FROM Usuarios WHERE FirebaseUID = @FirebaseUID", miConexion))
                    {
                        miComando.Parameters.AddWithValue("@FirebaseUID", firebaseUID);

                        using (SqlDataReader miLector = miComando.ExecuteReader())
                        {
                            if (miLector.Read())
                            {
                                usuario = new Usuario
                                {
                                    IdUsuario = (int)miLector["IdUsuario"],
                                    FirebaseUID = (string)miLector["FirebaseUID"],
                                    NombreUsuario = (string)miLector["NombreUsuario"],
                                    CorreoElectronico = (string)miLector["CorreoElectronico"],
                                    FotoPerfil = miLector["FotoPerfil"] != DBNull.Value ? (string)miLector["FotoPerfil"] : null,
                                    FechaRegistro = (DateTime)miLector["FechaRegistro"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario", ex);
            }

            return usuario;
        }

    }
}

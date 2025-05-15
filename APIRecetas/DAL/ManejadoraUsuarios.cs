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

        public static Usuario InsertaUsuario(string firebaseUID, string nombreUsuario, string correoElectronico)
        {
            Usuario usuario = null;

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();

                    using (SqlCommand miComando = new SqlCommand(
                        "INSERT INTO Usuarios (FirebaseUID, NombreUsuario, CorreoElectronico, FechaRegistro) " +
                        "OUTPUT INSERTED.IdUsuario, INSERTED.FirebaseUID, INSERTED.NombreUsuario, INSERTED.CorreoElectronico, INSERTED.FechaRegistro " +
                        "VALUES (@FirebaseUID, @NombreUsuario, @CorreoElectronico, @FechaRegistro)", miConexion))
                    {
                        miComando.Parameters.AddWithValue("@FirebaseUID", firebaseUID);
                        miComando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        miComando.Parameters.AddWithValue("@CorreoElectronico", correoElectronico);
                        miComando.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                        using (SqlDataReader reader = miComando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    IdUsuario = (int)reader["IdUsuario"],
                                    FirebaseUID = (string)reader["FirebaseUID"],
                                    NombreUsuario = (string)reader["NombreUsuario"],
                                    CorreoElectronico = (string)reader["CorreoElectronico"],
                                    FechaRegistro = (DateTime)reader["FechaRegistro"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el usuario", ex);
            }

            return usuario;
        }

        public static int EliminarUsuarioPorUID(string firebaseUID)
        {
            int filasAfectadas = 0;
            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();

                    using (SqlCommand miComando = new SqlCommand(
                        "DELETE FROM Usuarios WHERE FirebaseUID = @FirebaseUID", miConexion))
                    {
                        miComando.Parameters.AddWithValue("@FirebaseUID", firebaseUID);

                        // Ejecutamos el comando y devolvemos true si se eliminó algún registro
                        filasAfectadas = miComando.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario", ex);
            }

            return filasAfectadas;
        }

    }
}
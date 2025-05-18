using ClasesRecetas;
using DAL.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manejadoras
{
    public class ManejadoraUsuarios
    {
        public static DTOUsuario ObtieneUsuarioSimplificadoPorUID(string firebaseUID)
        {
            DTOUsuario usuario = null;

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
                                usuario = new DTOUsuario
                                {
                                    IdUsuario = (int)miLector["IdUsuario"],
                                    FechaRegistro = (DateTime)miLector["FechaRegistro"],
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

        public static Usuario ObtieneUsuarioPorUID(string firebaseUID)
        {
            Usuario usuario = null;

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();

                    using (SqlCommand miComando = new SqlCommand(
                        "SELECT IdUsuario, FirebaseUID, CorreoElectronico, NombreUsuario, FechaRegistro " +
                        "FROM Usuarios WHERE FirebaseUID = @FirebaseUID", miConexion))
                    {
                        miComando.Parameters.AddWithValue("@FirebaseUID", firebaseUID);

                        using (SqlDataReader reader = miComando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    IdUsuario = (int)reader["IdUsuario"],
                                    FirebaseUID = (string)reader["FirebaseUID"],
                                    CorreoElectronico = (string)reader["CorreoElectronico"],
                                    NombreUsuario = (string)reader["NombreUsuario"],
                                    FechaRegistro = (DateTime)reader["FechaRegistro"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario por FirebaseUID", ex);
            }

            return usuario;
        }

        public static Usuario InsertaUsuario(UsuarioInsert usuarioInsert)
        {
            Usuario usuario = null;

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();

                    using (SqlCommand miComando = new SqlCommand(
                        "INSERT INTO Usuarios (FirebaseUID, CorreoElectronico, NombreUsuario, FechaRegistro) " +
                        "OUTPUT INSERTED.IdUsuario, INSERTED.FirebaseUID, INSERTED.CorreoElectronico, INSERTED.NombreUsuario, INSERTED.FechaRegistro " +
                        "VALUES (@FirebaseUID, @CorreoElectronico, @NombreUsuario, @FechaRegistro)", miConexion))
                    {
                        miComando.Parameters.AddWithValue("@FirebaseUID", usuarioInsert.FirebaseUID);
                        miComando.Parameters.AddWithValue("@CorreoElectronico", usuarioInsert.CorreoElectronico);
                        miComando.Parameters.AddWithValue("@NombreUsuario", usuarioInsert.NombreUsuario);
                        miComando.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                        using (SqlDataReader reader = miComando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    IdUsuario = (int)reader["IdUsuario"],
                                    FirebaseUID = (string)reader["FirebaseUID"],
                                    CorreoElectronico = (string)reader["CorreoElectronico"],
                                    NombreUsuario = (string)reader["NombreUsuario"],
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
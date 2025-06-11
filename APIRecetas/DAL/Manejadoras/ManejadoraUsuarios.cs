using ClasesRecetas;
using DAL.DTO.DTOUsuario;
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
        /// <summary>
        /// Obtiene un usuario simplificado (IdUsuario y FechaRegistro) a partir de su FirebaseUID.
        /// </summary>
        /// <param name="firebaseUID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Obtiene un usuario completo a partir de su FirebaseUID.
        /// </summary>
        /// <param name="firebaseUID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Inserta un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="usuarioInsert"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Usuario InsertaUsuario(UsuarioInsert usuarioInsert)
        {
            Usuario usuario = null;

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();

                    using (SqlCommand miComando = new SqlCommand(
                        "INSERT INTO Usuarios (FirebaseUID, CorreoElectronico, NombreUsuario, FechaRegistro, FotoPerfil) " +
                        "OUTPUT INSERTED.IdUsuario, INSERTED.FirebaseUID, INSERTED.CorreoElectronico, INSERTED.NombreUsuario, INSERTED.FechaRegistro, INSERTED.FotoPerfil " +
                        "VALUES (@FirebaseUID, @CorreoElectronico, @NombreUsuario, @FechaRegistro, @FotoPerfil)", miConexion))
                    {
                        miComando.Parameters.AddWithValue("@FirebaseUID", usuarioInsert.FirebaseUID);
                        miComando.Parameters.AddWithValue("@CorreoElectronico", usuarioInsert.CorreoElectronico);
                        miComando.Parameters.AddWithValue("@NombreUsuario", usuarioInsert.NombreUsuario);
                        miComando.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                        miComando.Parameters.AddWithValue("@FotoPerfil", usuarioInsert.FotoPerfil);

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
                                    FechaRegistro = (DateTime)reader["FechaRegistro"],
                                    FotoPerfil = (string)reader["FotoPerfil"] as string
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

        /// <summary>
        /// Actualiza un usuario en la base de datos por su FirebaseUID.
        /// </summary>
        /// <param name="usuarioUpdate">Objeto con datos a actualizar</param>
        /// <returns>Usuario actualizado</returns>
        /// <exception cref="Exception"></exception>
        public static Usuario UpdateUsuarioPorUID(DTOUpdateUsuario usuarioUpdate)
        {
            Usuario usuario = null;

            try
            {
                using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    miConexion.Open();

                    using (SqlCommand miComando = new SqlCommand(
                        "UPDATE Usuarios " +
                        "SET NombreUsuario = @NombreUsuario, FotoPerfil = @FotoPerfil " +
                        "OUTPUT INSERTED.IdUsuario, INSERTED.FirebaseUID, INSERTED.CorreoElectronico, INSERTED.NombreUsuario, INSERTED.FechaRegistro, INSERTED.FotoPerfil " +
                        "WHERE FirebaseUID = @FirebaseUID", miConexion))
                    {
                        miComando.Parameters.AddWithValue("@FirebaseUID", usuarioUpdate.UID);
                        miComando.Parameters.AddWithValue("@NombreUsuario", usuarioUpdate.NombreUsuario);
                        miComando.Parameters.AddWithValue("@FotoPerfil", usuarioUpdate.FotoPerfil);

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
                                    FechaRegistro = (DateTime)reader["FechaRegistro"],
                                    FotoPerfil = (string)reader["FotoPerfil"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario", ex);
            }

            return usuario;
        }

    }

}
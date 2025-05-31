using DAL.DTO.DTOReceta;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manejadoras
{
    public class ManejadoraLikes
    {
        //#region Insert

        //public static bool AgregarLike(DTOLike like)
        //{
        //    bool estaAgregado = false;

        //    try
        //    {
        //        string consultaExiste = "SELECT COUNT(*) FROM Likes WHERE IdReceta = @idReceta AND IdUsuario = @idUsuario";

        //        using (SqlConnection conexion = new SqlConnection(Conexion.CadenaConexion()))
        //        {
        //            conexion.Open();

        //            using (SqlCommand comandoExiste = new SqlCommand(consultaExiste, conexion))
        //            {
        //                comandoExiste.Parameters.AddWithValue("@idReceta", like.IdReceta);
        //                comandoExiste.Parameters.AddWithValue("@idUsuario", like.IdUsuario);

        //                int count = (int)comandoExiste.ExecuteScalar();

        //                if (count == 0)
        //                {
        //                    string consultaInsertar = "INSERT INTO Likes (IdReceta, IdUsuario) VALUES (@idReceta, @idUsuario)";

        //                    using (SqlCommand comandoInsertar = new SqlCommand(consultaInsertar, conexion))
        //                    {
        //                        comandoInsertar.Parameters.AddWithValue("@idReceta", like.IdReceta);
        //                        comandoInsertar.Parameters.AddWithValue("@idUsuario", like.IdUsuario);

        //                        int filasAfectadas = comandoInsertar.ExecuteNonQuery();
        //                        estaAgregado = filasAfectadas > 0;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al agregar like", ex);
        //    }

        //    return estaAgregado;
        //}

        //#endregion

        //#region Delete

        //public static void EliminarLike(DTOLike like)
        //{
        //    try
        //    {
        //        using (SqlConnection miConexion = new SqlConnection(Conexion.CadenaConexion()))
        //        {
        //            miConexion.Open();

        //            string query = "DELETE FROM Likes WHERE IdUsuario = @idUsuario AND IdReceta = @idReceta";

        //            using (SqlCommand miComando = new SqlCommand(query, miConexion))
        //            {
        //                miComando.Parameters.AddWithValue("@idUsuario", like.IdUsuario);
        //                miComando.Parameters.AddWithValue("@idReceta", like.IdReceta);
        //                miComando.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al eliminar like", ex);
        //    }
        //}

        //#endregion

        public static bool ToggleLike(DTOLike like)
        {
            bool estadoLike = false;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    conexion.Open();

                    // Comprobamos si existe el like usando JOIN con Usuarios para filtrar por FirebaseUID
                    string consultaExiste = @"
                            SELECT COUNT(*) 
                            FROM Likes l
                            INNER JOIN Usuarios u ON l.IdUsuario = u.IdUsuario
                            WHERE l.IdReceta = @idReceta AND u.FirebaseUID = @uid";

                    using (SqlCommand comandoExiste = new SqlCommand(consultaExiste, conexion))
                    {
                        comandoExiste.Parameters.AddWithValue("@idReceta", like.IdReceta);
                        comandoExiste.Parameters.AddWithValue("@uid", like.Uid);

                        int count = (int)comandoExiste.ExecuteScalar();

                        if (count > 0)
                        {
                            // Si existe, eliminar el like
                            string consultaEliminar = @"
                                DELETE l
                                FROM Likes l
                                INNER JOIN Usuarios u ON l.IdUsuario = u.IdUsuario
                                WHERE l.IdReceta = @idReceta AND u.FirebaseUID = @uid";

                            using (SqlCommand comandoEliminar = new SqlCommand(consultaEliminar, conexion))
                            {
                                comandoEliminar.Parameters.AddWithValue("@idReceta", like.IdReceta);
                                comandoEliminar.Parameters.AddWithValue("@uid", like.Uid);
                                comandoEliminar.ExecuteNonQuery();
                            }

                            estadoLike = false; // Se eliminó el like
                        }
                        else
                        {
                            // Si no existe, agregar el like
                            // Para insertar necesitamos IdUsuario, así que primero lo buscamos
                            string consultaIdUsuario = "SELECT IdUsuario FROM Usuarios WHERE FirebaseUID = @uid";

                            int idUsuario;

                            using (SqlCommand comandoIdUsuario = new SqlCommand(consultaIdUsuario, conexion))
                            {
                                comandoIdUsuario.Parameters.AddWithValue("@uid", like.Uid);
                                object result = comandoIdUsuario.ExecuteScalar();
                                if (result == null)
                                {
                                    throw new Exception("No se encontró el usuario con ese UID");
                                }
                                idUsuario = (int)result;
                            }

                            string consultaInsertar = "INSERT INTO Likes (IdReceta, IdUsuario) VALUES (@idReceta, @idUsuario)";

                            using (SqlCommand comandoInsertar = new SqlCommand(consultaInsertar, conexion))
                            {
                                comandoInsertar.Parameters.AddWithValue("@idReceta", like.IdReceta);
                                comandoInsertar.Parameters.AddWithValue("@idUsuario", idUsuario);
                                comandoInsertar.ExecuteNonQuery();
                            }

                            estadoLike = true; // Se agregó el like
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al realizar toggle de like", ex);
            }

            return estadoLike;
        }

    }

}

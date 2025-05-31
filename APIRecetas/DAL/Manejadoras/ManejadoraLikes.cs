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
        /// <summary>
        /// Le hace toggle a un like de una receta. Si el like ya existe, lo elimina; si no, lo agrega.
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool ToggleLike(DTOLike like)
        {
            bool estadoLike = false;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.CadenaConexion()))
                {
                    conexion.Open();

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

                            estadoLike = true;
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

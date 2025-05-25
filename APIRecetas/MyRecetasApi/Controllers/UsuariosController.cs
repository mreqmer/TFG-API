using ClasesRecetas;
using DAL.DTO;
using DAL.Manejadoras;
using Microsoft.AspNetCore.Mvc;

namespace MyRecetasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        // GET: api/Usuarios/{firebaseUID}
        [HttpGet("{firebaseUID}")]
        public IActionResult GetUsuarioInfoAdicional(string firebaseUID)
        {
            try
            {
                DTOUsuario usuario = ManejadoraUsuarios.ObtieneUsuarioSimplificadoPorUID(firebaseUID);

                if (usuario == null)
                {
                    return NotFound(new { message = "No se encontró el usuario con ese UID." });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error al obtener el usuario.", error = ex.Message });
            }
        }

        // POST: api/Usuarios/Nuevo
        [HttpPost("Nuevo")]
        public IActionResult PostUsuario([FromBody] UsuarioInsert usuario)
        {
            try
            {
                if (string.IsNullOrEmpty(usuario.FirebaseUID) || string.IsNullOrEmpty(usuario.NombreUsuario) || string.IsNullOrEmpty(usuario.CorreoElectronico))
                {
                    return BadRequest(new { message = "Debe introducir todos los datos." });
                }

                // Comprobar si ya existe usuario con ese FirebaseUID
                var usuarioExistente = ManejadoraUsuarios.ObtieneUsuarioPorUID(usuario.FirebaseUID);
                if (usuarioExistente != null)
                {
                    // Ya existe, devolver el usuario existente (o algún mensaje personalizado)
                    return Ok(usuarioExistente);
                }

                // Si no existe, insertar nuevo usuario
                Usuario newUser = ManejadoraUsuarios.InsertaUsuario(usuario);
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error al insertar el usuario.", error = ex.Message });
            }
        }

        // PUT: api/Usuarios/Editar/{firebaseUID}
        [HttpPut("Editar/{firebaseUID}")]
        public IActionResult EditarUsuario(string firebaseUID, [FromBody] DTOUpdateUsuario usuarioEditado)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioEditado.NombreUsuario) || string.IsNullOrEmpty(usuarioEditado.CorreoElectronico))
                {
                    return BadRequest(new { message = "Debe proporcionar un nombre de usuario y un correo electrónico válidos." });
                }

                bool actualizado = ManejadoraUsuarios.ActualizarUsuario(firebaseUID, usuarioEditado);

                if (!actualizado)
                {
                    return NotFound(new { message = "No se encontró un usuario con ese UID para actualizar." });
                }

                return Ok(new { message = "Usuario actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error al actualizar el usuario.", error = ex.Message });
            }
        }

        // DELETE: api/Usuarios/Borrar/{firebaseUID}
        [HttpDelete("Borrar/{firebaseUID}")]
        public IActionResult DeleteUsuario(string firebaseUID)
        {
            try
            {
                int filasAfectadas = ManejadoraUsuarios.EliminarUsuarioPorUID(firebaseUID);

                if (filasAfectadas > 0)
                {
                    return Ok($"Usuario eliminado con éxito. {filasAfectadas} filas afectadas.");
                }
                else
                {
                    return NotFound("No se encontró el usuario con ese UID.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al eliminar el usuario." + ex.Message );
            }
        }
    }



}
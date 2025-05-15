using ClasesRecetas;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace MyRecetasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        // GET: api/Usuarios/{firebaseUID}
        [HttpGet("{firebaseUID}")]
        public IActionResult GetUsuarioByFirebaseUID(string firebaseUID)
        {
            try
            {
                Usuario usuario = ManejadoraUsuarios.ObtieneUsuarioPorUID(firebaseUID);

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
                // Verificar que los campos necesarios estén presentes
                if (string.IsNullOrEmpty(usuario.FirebaseUID) || string.IsNullOrEmpty(usuario.NombreUsuario) || string.IsNullOrEmpty(usuario.CorreoElectronico))
                {
                    return BadRequest(new { message = "Debe introducir todos los datos." });
                }

                // Insertar el nuevo usuario
                Usuario newUser = ManejadoraUsuarios.InsertaUsuario(usuario.FirebaseUID, usuario.NombreUsuario, usuario.CorreoElectronico);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error al insertar el usuario.", error = ex.Message });
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
using ClasesRecetas;
using DAL.DTO.DTOUsuario;
using DAL.Manejadoras;
using Microsoft.AspNetCore.Mvc;

namespace MyRecetasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        // GET: api/Usuarios/{firebaseUID}
        /// <summary>
        /// Obtiene la información adicional de un usuario por su Firebase UID.
        /// </summary>
        /// <param name="firebaseUID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Inserta un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Actualiza la información de un usuario existente por su UID.
        /// </summary>
        /// <param name="usuarioUpdate"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public IActionResult UpdateUsuario([FromBody] DTOUpdateUsuario usuarioUpdate)
        {
            if (usuarioUpdate == null || string.IsNullOrEmpty(usuarioUpdate.UID))
            {
                return BadRequest("Datos inválidos o UID no especificado.");
            }

            try
            {
                var usuarioActualizado = ManejadoraUsuarios.UpdateUsuarioPorUID(usuarioUpdate);

                if (usuarioActualizado == null)
                {
                    return NotFound("Usuario no encontrado.");
                }

                return Ok(usuarioActualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar usuario: {ex.Message}");
            }
        }

    }
}
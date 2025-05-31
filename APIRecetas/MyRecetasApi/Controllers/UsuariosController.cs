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

    //    // PUT: api/Usuarios/Editar/{firebaseUID}
    //    /// <summary>
    //    /// Actualiza la información de un usuario existente por su Firebase UID.
    //    /// </summary>
    //    /// <param name="firebaseUID"></param>
    //    /// <param name="usuarioEditado"></param>
    //    /// <returns></returns>
    //    [HttpPut("Editar/{firebaseUID}")]
    //    public IActionResult EditarUsuario(string firebaseUID, [FromBody] DTOUpdateUsuario usuarioEditado)
    //    {
    //        try
    //        {
    //            if (string.IsNullOrEmpty(usuarioEditado.NombreUsuario) || string.IsNullOrEmpty(usuarioEditado.CorreoElectronico))
    //            {
    //                return BadRequest(new { message = "Debe proporcionar un nombre de usuario y un correo electrónico válidos." });
    //            }

    //            bool actualizado = ManejadoraUsuarios.ActualizarUsuario(firebaseUID, usuarioEditado);

    //            if (!actualizado)
    //            {
    //                return NotFound(new { message = "No se encontró un usuario con ese UID para actualizar." });
    //            }

    //            return Ok(new { message = "Usuario actualizado correctamente." });
    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(StatusCodes.Status500InternalServerError,
    //                new { message = "Error al actualizar el usuario.", error = ex.Message });
    //        }
    //    }
    }
}
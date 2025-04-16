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
    }
}

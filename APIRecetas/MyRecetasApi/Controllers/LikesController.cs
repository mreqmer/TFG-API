using ClasesRecetas;
using DAL.DTO;
using DAL.Manejadoras;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MyRecetasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {

        //POST: api/Likes/toggle
        [HttpPost("toggle")]
        public IActionResult ToggleLike([FromBody] DTOLike like)
        {
            if (like == null || like.IdReceta <= 0 || like.Uid.IsNullOrEmpty())
            {
                return BadRequest("Datos inválidos.");
            }

            try
            {
                bool likeEstado = ManejadoraLikes.ToggleLike(like);

                return Ok(new
                {
                    Success = true,
                    LikeActivo = likeEstado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

    }
}

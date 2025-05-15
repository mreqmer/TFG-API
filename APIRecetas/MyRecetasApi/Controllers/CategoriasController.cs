using ClasesRecetas;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace MyRecetasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        [HttpGet("listado")]
        public IActionResult GetCategorias()
        {
            try
            {
                // Intentamos obtener el listado de categorías
                List<Categoria> categorias = ManejadoraCategorias.ObtieneListadoCategorias();

                // Si no encontramos categorías, retornamos un error 404
                if (categorias == null || !categorias.Any())
                {
                    return NotFound(new { message = "No se encontraron categorías." });
                }

                // Si todo va bien, retornamos el listado con un 200 OK
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, retornamos un error 500
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener las categorías.", error = ex.Message });
            }
        }
    }
}

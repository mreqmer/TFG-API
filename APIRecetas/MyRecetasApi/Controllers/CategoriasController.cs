using ClasesRecetas;
using DAL.Manejadoras;
using Microsoft.AspNetCore.Mvc;

namespace MyRecetasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        //Get: api/categorias/listado
        /// <summary>
        /// Obtiene un listado de todas las categorías disponibles.
        /// </summary>
        /// <returns></returns>
        [HttpGet("listado")]
        public IActionResult GetCategorias()
        {
            try
            {
                List<Categoria> categorias = ManejadoraCategorias.ObtieneListadoCategorias();

                if (categorias == null || !categorias.Any())
                {
                    return NotFound(new { message = "No se encontraron categorías." });
                }

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener las categorías.", error = ex.Message });
            }
        }
    }
}

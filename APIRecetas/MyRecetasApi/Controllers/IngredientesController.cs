using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClasesRecetas;
using DAL.Manejadoras;

namespace MyRecetasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientesController : ControllerBase
    {
        // GET: api/<Ingredientes>
        /// <summary>
        /// Obtiene un listado de todos los ingredientes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Ingrediente> ingredientes = ManejadoraIngredientes.ObtieneListadoIngredientes();

                if (ingredientes == null || !ingredientes.Any())
                {
                    return NotFound(new { message = "No se encontraron ingredientes." });
                }

                return Ok(ingredientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener los ingredientes.", error = ex.Message });
            }
        }
        // GET: api/<Ingredientes>/{idIngrediente}
        /// <summary>
        /// Obtiene un ingrediente específico por su ID.
        /// </summary>
        /// <param name="idIngrediente"></param>
        /// <returns></returns>
        [HttpGet("{idIngrediente}")]
        public IActionResult Get(int idIngrediente)
        {
            try
            {
                Ingrediente ingredientes = ManejadoraIngredientes.ObtieneIngrediente(idIngrediente);

                if (ingredientes == null)
                {
                    return NotFound(new { message = "No se encontraron ingredientes." });
                }

                return Ok(ingredientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener los ingredientes.", error = ex.Message });
            }
        }

        // GET: api/<Ingredientes>/Buscar
        /// <summary>
        /// Busca ingredientes por nombre.
        /// </summary>
        /// <param name="busquedaNombre"></param>
        /// <returns></returns>
        [HttpGet("Buscar")]
        public IActionResult BuscarIngredientes([FromQuery] string busquedaNombre)
        {
            try
            {  
                List<Ingrediente> ingredientes = ManejadoraIngredientes.ObtieneIngredienteBuscador(busquedaNombre);

                if (ingredientes == null || !ingredientes.Any())
                {
                    return NotFound(new { message = "No se encontraron ingredientes." });
                }

                return Ok(ingredientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener los ingredientes.", error = ex.Message });
            }
        }
    }
}
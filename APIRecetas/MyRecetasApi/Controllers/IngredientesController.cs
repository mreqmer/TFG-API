using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClasesRecetas;
using DAL;

namespace MyRecetasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientesController : ControllerBase
    {
        // Ingredientes/Categoria
        //Solo get, no va a crear el usuario cosas

        // GET: api/<Ingredientes>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // Intentamos obtener el listado de ingredientes
                List<Ingrediente> ingredientes = ManejadoraIngredientes.ObtieneListadoIngredientes();

                // Si no encontramos ingredientes, retornamos un error 404
                if (ingredientes == null || !ingredientes.Any())
                {
                    return NotFound(new { message = "No se encontraron ingredientes." });
                }

                // Si todo va bien, retornamos el listado con un 200 OK
                return Ok(ingredientes);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, retornamos un error 500
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener los ingredientes.", error = ex.Message });
            }
        }
        // GET: api/<Ingredientes>/{idIngrediente}
        [HttpGet("{idIngrediente}")]
        public IActionResult Get(int idIngrediente)
        {
            try
            {
                // Intentamos obtener el listado de ingredientes
                Ingrediente ingredientes = ManejadoraIngredientes.ObtieneIngrediente(idIngrediente);

                // Si no encontramos ingredientes, retornamos un error 404
                if (ingredientes == null)
                {
                    return NotFound(new { message = "No se encontraron ingredientes." });
                }

                // Si todo va bien, retornamos el listado con un 200 OK
                return Ok(ingredientes);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, retornamos un error 500
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener los ingredientes.", error = ex.Message });
            }
        }

        // GET: api/<Ingredientes>/Buscar
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

        // GET: api/<Ingredientes>/{categoria}
        [HttpGet("Categoria/{Categoria}")]
        public IActionResult GetCategoria(string Categoria)
        {
            try
            {
                // Intentamos obtener el listado de ingredientes
                List<Ingrediente> ingredientes = ManejadoraIngredientes.ObtieneListadoIngredientesCategorias(Categoria);

                // Si no encontramos ingredientes, retornamos un error 404
                if (ingredientes == null || !ingredientes.Any())
                {
                    return NotFound(new { message = "No se encontraron ingredientes." });
                }

                // Si todo va bien, retornamos el listado con un 200 OK
                return Ok(ingredientes);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, retornamos un error 500
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener los ingredientes.", error = ex.Message });
            }
        }
    }
}
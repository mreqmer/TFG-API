using ClasesRecetas;
using DAL;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;


namespace MyRecetasApi.Controllers
{
    public class RecetasController : Controller
    {
        // GET: api/<Recetas>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // Intentamos obtener el listado de recetas
                List<Receta> listadoRecetas = ManejadoraRecetas.ObtieneListadoRecetas();

                // Si no encontramos recetas, retornamos un error 404
                if (listadoRecetas == null || !listadoRecetas.Any())
                {
                    return NotFound(new { message = "No se encontraron recetas." });
                }

                // Si todo va bien, retornamos el listado con un 200 OK
                return Ok(listadoRecetas);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, retornamos un error 500
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener las recetas.", error = ex.Message });
            }
        }

        // GET: api/Recetas/{idReceta}
        [HttpGet("{idReceta}")]
        public IActionResult GetRecetaById(int idReceta)
        {
            try
            {
                // Intentamos obtener la receta por ID
                RecetaDTO receta = ManejadoraRecetas.ObtieneRecetaID(idReceta);

                // Si no encontramos la receta, retornamos un error 404
                if (receta == null)
                {
                    return NotFound(new { message = $"No se encontró la receta con ID {idReceta}." });
                }

                // Esto sirve únicamente para que se ordene el objeto en el json
                var response = new
                {
                    receta = new
                    {
                        receta.IdReceta,
                        receta.NombreReceta,
                        receta.Descripcion,
                        receta.TiempoPreparacion,
                        receta.Dificultad,
                        receta.FechaCreacion
                    },
                    pasos = receta.Pasos,
                    ingredientes = receta.Ingredientes,
                    categorias = receta.Categorias
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, retornamos un error 500
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener la receta.", error = ex.Message });
            }
        }


        // GET: api/Recetas/nombre/{NombreReceta}
        [HttpGet("nombre/{NombreReceta}")]
        public IActionResult GetRecetaByName(string NombreReceta)
        {
            try
            {
                // Intentamos obtener la receta por nombre
                RecetaDTO receta = ManejadoraRecetas.ObtieneRecetaNombre(NombreReceta);

                // Si no encontramos la receta, retornamos un error 404
                if (receta == null)
                {
                    return NotFound(new { message = "No se encontró la receta." });
                }

                // Esto sirve únicamente para ordenar el objeto
                var response = new
                {
                    receta = new
                    {
                        receta.IdReceta,
                        receta.NombreReceta,
                        receta.Descripcion,
                        receta.TiempoPreparacion,
                        receta.Dificultad,
                        receta.FechaCreacion
                    },
                    pasos = receta.Pasos,
                    ingredientes = receta.Ingredientes,
                    categorias = receta.Categorias
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, retornamos un error 500
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener la receta.", error = ex.Message });
            }
        }




    }
}

using ClasesRecetas;
using DAL;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;


namespace MyRecetasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecetasController : ControllerBase
    {
        // GET: api/<Recetas>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<RecetaUsuario> listadoRecetas = ManejadoraRecetas.ObtieneListadoRecetas();

                if (listadoRecetas == null || !listadoRecetas.Any())
                {
                    return NotFound(new { message = "No se encontraron recetas." });
                }

                return Ok(listadoRecetas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener las recetas.", error = ex.Message });
            }
        }

        // GET: api/<Recetas>/{idReceta}
        [HttpGet("{idReceta}")]
        public IActionResult GetRecetaById(int idReceta)
        {
            try
            {
                RecetaDTO receta = ManejadoraRecetas.ObtieneRecetaID(idReceta);

                if (receta == null)
                {
                    return NotFound(new { message = $"No se encontró la receta con ID {idReceta}." });
                }

                var response = new
                {
                    receta = new
                    {
                        receta.IdReceta,
                        receta.NombreReceta,
                        receta.Descripcion,
                        receta.TiempoPreparacion,
                        receta.Dificultad,
                        receta.FechaCreacion,
                        receta.FotoReceta,
                        receta.NombreUsuario
                    },
                    pasos = receta.Pasos,
                    ingredientes = receta.Ingredientes,
                    categorias = receta.Categorias
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener la receta.", error = ex.Message });
            }
        }

        // GET: api/<Recetas>/creador/{idCreador}
        [HttpGet("creador/{idCreador}")]
        public IActionResult GetRecetasByCreador(int idCreador)
        {
            try
            {
                List<RecetaUsuario> recetas = ManejadoraRecetas.ObtieneRecetasPorIdCreador(idCreador);

                if (recetas == null || !recetas.Any())
                {
                    return NotFound(new { message = $"No se encontraron recetas para el creador con ID {idCreador}." });
                }

                var response = recetas.Select(receta => new
                {
                    receta.IdReceta,
                    receta.NombreReceta,
                    receta.Descripcion,
                    receta.TiempoPreparacion,
                    receta.Dificultad,
                    receta.FechaCreacion,
                    receta.FotoReceta,
                    receta.NombreUsuario
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener las recetas por IdCreador.", error = ex.Message });
            }
        }

        // GET: api/<Recetas>/nombre/{NombreReceta}
        [HttpGet("nombre/{NombreReceta}")]
        public IActionResult GetRecetaByName(string NombreReceta)
        {
            try
            {
                RecetaDTO receta = ManejadoraRecetas.ObtieneRecetaNombre(NombreReceta);

                if (receta == null)
                {
                    return NotFound(new { message = "No se encontró la receta." });
                }

                var response = new
                {
                    receta = new
                    {
                        receta.IdReceta,
                        receta.NombreReceta,
                        receta.Descripcion,
                        receta.TiempoPreparacion,
                        receta.Dificultad,
                        receta.FechaCreacion,
                        receta.FotoReceta,
                        receta.NombreUsuario
                    },
                    pasos = receta.Pasos,
                    ingredientes = receta.Ingredientes,
                    categorias = receta.Categorias
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener la receta.", error = ex.Message });
            }
        }

        [HttpPost("add")]
        public IActionResult AddReceta([FromBody] DTONuevaReceta receta)
        {
            try
            {
                // Insertar receta completa
                DTONuevaReceta recetaInsertada = ManejadoraRecetas.InsertaRecetaCompleta(receta);

                // Devolver la receta insertada
                return Ok(new
                {
                    receta = new
                    {
                        recetaInsertada.IdReceta,
                        recetaInsertada.NombreReceta,
                        recetaInsertada.Descripcion,
                        recetaInsertada.TiempoPreparacion,
                        recetaInsertada.Dificultad,
                        recetaInsertada.FechaCreacion,
                        recetaInsertada.FotoReceta,
                        recetaInsertada.IdCreador,
                        pasos = recetaInsertada.Pasos,
                        ingredientes = recetaInsertada.Ingredientes,
                        categorias = recetaInsertada.Categorias
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al insertar la receta.", error = ex.Message });
            }
        }


    }


}
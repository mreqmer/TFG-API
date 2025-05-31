using ClasesRecetas;
using DAL.DTO;
using DAL.Manejadoras;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyRecetasApi.Models;


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

        // GET: api/<Recetas>/RecetasLikes/uid
        [HttpGet ("RecetasLikes/{uid}")]
        public IActionResult GetRecetasUsuarioLikes(string uid)
        {
            try
            {
                List<RecetaUsuarioLike> listadoRecetas = ManejadoraRecetas.ObtieneListadoRecetasConLike(uid);

                if (listadoRecetas == null || listadoRecetas.Count == 0)
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

        // GET: api/<Recetas>/RecetasLikes/{uid}/{busqueda}
        [HttpGet("RecetasLikes/Busqueda/{uid}/{busqueda}")]
        public IActionResult GetRecetasUsuarioLikesPorNombre(string uid, string busqueda)
        {
            try
            {
                List<RecetaUsuarioLike> listadoRecetas = ManejadoraRecetas.ObtieneRecetasConLikePorNombre(uid, busqueda);

                if (listadoRecetas == null || listadoRecetas.Count == 0)
                {
                    return NotFound(new { message = "No se encontraron recetas que coincidan con la búsqueda." });
                }

                return Ok(listadoRecetas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener las recetas con like por nombre.", error = ex.Message });
            }
        }

        [HttpGet("RecetasLikes/Busqueda/{uid}")]
        public IActionResult GetRecetasUsuarioLikesFiltrado(
                string uid,
                [FromQuery] string busqueda = "",
                [FromQuery] string? categoria = null,
                [FromQuery] int? tiempo = null,
                [FromQuery] string? dificultad = null,
                [FromQuery] List<string>? ingredientes = null
        )
        {
            try
            {

                var listadoRecetas = ManejadoraRecetas.ObtieneRecetasConLikeFiltrado(uid, busqueda, categoria, tiempo, dificultad, ingredientes);

                if (listadoRecetas == null || listadoRecetas.Count == 0)
                {
                    return NotFound(new { message = "No se encontraron recetas que coincidan con la búsqueda." });
                }

                return Ok(listadoRecetas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un error al obtener las recetas con like filtradas.", error = ex.Message });
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

        [HttpPost("Favorita")]
        public IActionResult GetRecetaDetalladaLike(DTOGetRecetaDetalladaLike model)
        {
            try
            {
                if (model.Uid.IsNullOrEmpty() || model.IdReceta <= 0)
                {
                    return BadRequest(new { mensaje = "Rellena los campos" });
                }

                DTORecetaDetalladaLike receta = ManejadoraRecetas.ObtieneRecetaIDFavorito(model);



                if (receta.IdReceta == 0)
                {
                    return NotFound(new { mensaje = "No se encontró la receta" });
                }


                return Ok(receta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener la receta", error = ex.Message });
            }
        }

        [HttpGet("likes/{uid}")]
        public ActionResult<List<RecetaUsuarioLike>> GetRecetasLikedByUser(string uid)
        {
            try
            {
                List<RecetaUsuarioLike> recetasLiked = ManejadoraRecetas.ObtieneRecetasFavoritasPorUid(uid);
                return Ok(recetasLiked);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener las recetas liked por el usuario", detalle = ex.Message });
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

        [HttpDelete("Borrar")]
        public IActionResult DeleteReceta([FromQuery] string uid, [FromQuery] int idReceta)
        {
            try
            {
                ManejadoraRecetas.BorrarRecetaPorIdYUid(uid, idReceta);

                return Ok(new { message = "Receta eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
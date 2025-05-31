using DAL.DTO.DTOReceta;
using DAL.Manejadoras;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace MyRecetasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecetasController : ControllerBase
    {
        #region Pruebas básicas
        // GET: api/<Recetas>
        /// <summary>
        /// Obtiene un listado de todas las recetas existentes.
        /// </summary>
        /// <returns>Listado de recetas o error si no se encuentran.</returns>
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

        //GET: api/<Recetas>/Receta/{idReceta}
        /// <summary>
        /// Obtiene los detalles de una receta por su ID.
        /// </summary>
        /// <param name="idReceta">ID de la receta</param>
        /// <returns>Receta detallada o error si no se encuentra.</returns>
        [HttpGet("{idReceta}")]
        public IActionResult GetRecetaById(int idReceta)
        {
            try
            {
                DTOReceta receta = ManejadoraRecetas.ObtieneRecetaID(idReceta);

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

        // GET: api/<Recetas>/nombre/{NombreReceta}
        /// <summary>
        /// Obtiene los detalles de una receta por su nombre.
        /// </summary>
        /// <param name="NombreReceta">Nombre de la receta</param>
        /// <returns>Receta detallada o error si no se encuentra.</returns>
        [HttpGet("nombre/{NombreReceta}")]
        public IActionResult GetRecetaByName(string NombreReceta)
        {
            try
            {
                DTOReceta receta = ManejadoraRecetas.ObtieneRecetaNombre(NombreReceta);

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
        #endregion

        #region Objeto con Fav

        // POST: api/<Recetas>/RecetaLikes
        /// <summary>
        /// Obtiene una receta detallada incluyendo información de si está marcada como favorita por el usuario.
        /// </summary>
        /// <param name="model">Modelo con UID e ID de receta</param>
        /// <returns>Receta detallada con favoritos o error.</returns>
        [HttpPost("RecetaLikes")]
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
        #endregion

        #region Listados con Fav

        //GET: api/<Recetas>/RecetaLikes/Listado/{uid}
        // <summary>
        /// Obtiene las recetas(con like y marcadas como favoritas).
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Listado de recetas favoritas.</returns>
        [HttpGet("RecetaLikes/Listado/{uid}")]
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

        /// <summary>
        /// Obtiene las recetas con like filtradas por búsqueda, categoría, dificultad, tiempo o ingredientes.
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="busqueda">Texto de búsqueda</param>
        /// <param name="categoria">Categoría</param>
        /// <param name="tiempo">Tiempo máximo</param>
        /// <param name="dificultad">Nivel de dificultad</param>
        /// <param name="ingredientes">Lista de ingredientes</param>
        /// <returns>Listado filtrado de recetas con like.</returns>
        [HttpGet("RecetaLikes/Listado/Busqueda/{uid}")]
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


        // GET: api/<Recetas>/RecetaLikes/ListadoFav/{uid}
        /// <summary>
        /// Obtiene las recetas favoritas del usuario (con like y marcadas como favoritas).
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Listado de recetas favoritas.</returns>
        [HttpGet("RecetaLikes/ListadoFav/{uid}")]
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
        /// <summary>
        /// Obtiene las recetas creadas por un usuario específico.
        /// </summary>
        /// <param name="idCreador">ID del creador</param>
        /// <returns>Listado de recetas simplificadas.</returns>
        [HttpGet("Creador/{idCreador}")]
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
        #endregion

        #region Borrar
        // POST: api/<Recetas>/like
        /// <summary>
        /// Elimina una receta según su ID y UID del creador.
        /// </summary>
        /// <param name="uid">UID del creador</param>
        /// <param name="idReceta">ID de la receta</param>
        /// <returns>Resultado de la operación.</returns>
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
        #endregion

        #region Add
        // POST: api/<Recetas>/add
        /// <summary>
        /// Añade una nueva receta a la base de datos.
        /// </summary>
        /// <param name="receta">Receta a insertar</param>
        /// <returns>Receta insertada con sus datos.</returns>
        [HttpPost("Add")]
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
        #endregion
    }
}
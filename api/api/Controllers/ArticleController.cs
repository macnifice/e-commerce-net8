using api.Business.Interfaces.Article;
using Microsoft.AspNetCore.Mvc;
using api.Models.Article;
using api.Models.Article.Response;
using api.Models.Article.Request;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{  
    [Route("api/v1/[controller]")]
    [ApiController]   
    public class ArticleController(IArticleService _articleService) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ArticleDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ArticleDto>>> GetAllArticles()
        {
            List<ArticleDto> response = await _articleService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("detail/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ArticleDto>> GetArticleById(int id)
        {
            try
            {
                ArticleDto response = await _articleService.GetbyId(id);
                if (response is null)
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Artículo no encontrado",
                        Detail = $"No se encontró el artículo con ID {id}"
                    });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al obtener el artículo",
                    Detail = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ArticleRsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<int>> CreateArticle(CreateOrEditArticleRqDto rq)
        {
            if (String.IsNullOrEmpty(rq.Name)
                && String.IsNullOrEmpty(rq.Code)
                && String.IsNullOrEmpty(rq.Description)
                && String.IsNullOrEmpty(rq.ImagePath)
                )
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Datos inválidos",
                    Detail = "La información proporcionada no es válida"
                });
            }

            int? response = await _articleService.CreateArticle(rq);
            if (response is null)
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Articulo ya existente",
                    Detail = $"Ya existe un articulo con el nombre '{rq.Name}'"
                });

            return Created($"/api/store/{response}", new ArticleRsDto
            {
                Id = response.Value,
                Message = "Created successfully"
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleRsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<int>> UpdateArticle(int id, CreateOrEditArticleRqDto rq)
        {
            int? response = await _articleService.Update(id, rq);
            if (response is null)
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Articulo de tienda no encontrado",
                    Detail = $"No se encontró la tienda con ID {id}"
                });

            return Ok(new ArticleRsDto
            {
                Id = response.Value,
                Message = "Updated successfully"
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleRsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult> DeleteArticle(int id)
        {
            try
            {
                int? response = await _articleService.Delete(id);
                if (response is null)
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Artículo no encontrado",
                        Detail = $"No se encontró el artículo con ID {id}"
                    });
                return Ok(new ArticleRsDto
                {
                    Id = response.Value,
                    Message = "Deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al eliminar el artículo",
                    Detail = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("assign/{storeId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ArticleStoreDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ArticleStoreDto>>> GetArticleStores(int storeId)
        {
            List<ArticleStoreDto> response = await _articleService.GetAllStores(storeId);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ArticleRsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<int>> AssignArticleStore(CreateOrEditArticleStoreDto rq)
        {
            try
            {
                if (rq.Date == DateTime.MinValue
                    || rq.Price <= 0
                    || rq.Stock < 0
                    || rq.StoreId <= 0
                    || rq.ArticleId <= 0)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Datos inválidos",
                        Detail = "La información proporcionada no es válida. Todos los campos son obligatorios y deben tener valores válidos."
                    });
                }

                int? response = await _articleService.AssaignArticleStore(rq);
                if (response is null)
                    return BadRequest(new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Artículo ya asignado",
                        Detail = $"El artículo ya está asignado a esta tienda"
                    });

                return Created($"/api/article/{response}", new ArticleRsDto
                {
                    Id = response.Value,
                    Message = "Article assigned to store successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al asignar artículo a tienda",
                    Detail = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("assign/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleRsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<int>> UpdateArticleStore(int id, CreateOrEditArticleStoreDto rq)
        {
            try
            {
                if (rq.Price <= 0 || rq.Stock < 0)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Datos inválidos",
                        Detail = "El precio debe ser mayor que cero y el stock no puede ser negativo"
                    });
                }

                int? response = await _articleService.UpdateArticleStore(id, rq);
                if (response is null)
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Asignación no encontrada",
                        Detail = $"No se encontró la asignación con ID {id}"
                    });

                return Ok(new ArticleRsDto
                {
                    Id = response.Value,
                    Message = "Article store assignment updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al actualizar la asignación",
                    Detail = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("assign/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleRsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult> DeleteArticleStore(int id)
        {
            try
            {
                int? response = await _articleService.DeleteArticleStore(id);
                if (response is null)
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Asignación no encontrada",
                        Detail = $"No se encontró la asignación con ID {id}"
                    });
                return Ok(new ArticleRsDto
                {
                    Id = response.Value,
                    Message = "Article store assignment deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al eliminar la asignación",
                    Detail = ex.Message
                });
            }
        }

    }
}

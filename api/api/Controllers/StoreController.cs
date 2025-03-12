using api.Business.Interfaces.Store;
using api.Data.Entities;
using api.Models.Store;
using api.Models.Store.Request;
using api.Models.Store.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StoreController(IStoreService _storeService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StoreDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<StoreDto>>> GetStores()
        {
            List<StoreDto> response = await _storeService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StoreDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<StoreDto>> GetStorebyId(int id)
        {
            StoreDto response = await _storeService.GetbyId(id);
            if (response is null)
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Tienda no encontrada",
                    Detail = $"No se encontró la tienda con ID {id}"
                });
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StoreRsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<int>> CreateStore(CreateOrEditStoreRqDto rq)
        {
            if (String.IsNullOrEmpty(rq.Name) && String.IsNullOrEmpty(rq.Address))
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Datos inválidos",
                    Detail = "La información proporcionada no es válida"
                });
            }

            int? response = await _storeService.Create(rq);
            if (response is null)
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Tienda existente",
                    Detail = $"Ya existe una tienda con el nombre '{rq.Name}'"
                });

            return Created($"/api/store/{response}", new StoreRsDto
            {
                Id = response.Value,
                Message = "Created successfully"
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StoreRsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<int>> UpdateStore(int id, CreateOrEditStoreRqDto rq)
        {
            int? response = await _storeService.Update(id, rq);
            if (response is null)
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Tienda no encontrada",
                    Detail = $"No se encontró la tienda con ID {id}"
                });

            return Ok(new StoreRsDto
            {
                Id = response.Value,
                Message = "Updated successfully"
            });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StoreRsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult> DeleteStore(int id)
        {
            int? response = await _storeService.Delete(id);
            if (response is null)
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Tienda no encontrada",
                    Detail = $"No se encontró la tienda con ID {id}"
                });
            return Ok(new StoreRsDto
            {
                Id = response.Value,
                Message = "Deleted successfully"
            });
        }
    }
}

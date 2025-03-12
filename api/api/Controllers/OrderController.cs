using api.Business.Interfaces.Order;
using api.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController(IOrderService _orderService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int userIdInt))
                {
                    return BadRequest(new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Usuario no válido",
                        Detail = "No se pudo identificar al usuario actual"
                    });
                }

                List<OrderDto> orders = await _orderService.GetOrdersByUserId(userIdInt);
                if (orders == null || orders.Count == 0)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Órdenes no encontradas",
                        Detail = "No se encontraron órdenes para este usuario"
                    });
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al obtener las órdenes",
                    Detail = ex.Message
                });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderRsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<OrderRsDto>> CreateOrder(List<CreateOrderRqDto> rq)
        {
            try
            {
                if (rq == null || rq.Count == 0)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Datos inválidos",
                        Detail = "No se proporcionaron artículos para la orden"
                    });
                }

                foreach (var item in rq)
                {
                    if (item.Quantity <= 0 || item.ArticleStoreId <= 0)
                    {
                        return BadRequest(new ProblemDetails
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "Datos inválidos",
                            Detail = "La cantidad debe ser mayor que cero y el ID del artículo debe ser válido"
                        });
                    }
                }

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int userIdInt))
                {
                    return BadRequest(new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Usuario no válido",
                        Detail = "No se pudo identificar al usuario actual"
                    });
                }

                OrderRsDto result = await _orderService.Create(rq, userIdInt);
                if (result == null)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Error al crear la orden",
                        Detail = "No se pudo crear la orden. Verifique el stock disponible de los artículos."
                    });
                }

                return Created($"/api/v1/order", result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error al crear la orden",
                    Detail = ex.Message
                });
            }
        }
    }
}

using api.Models.Order;

namespace api.Business.Interfaces.Order
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOrdersByUserId(int userId);
        Task<OrderRsDto> Create(List<CreateOrderRqDto> rq, int userId);
    }
}

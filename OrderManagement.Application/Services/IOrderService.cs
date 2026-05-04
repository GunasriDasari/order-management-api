using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int customerId, Dictionary<int, int> productQuantities);

        Task<Order?> GetOrderByIdAsync(int id);
    }
}

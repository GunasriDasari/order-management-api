using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(
            int customerId, 
            Dictionary<int, int> productQuantities,
            string? discountType,
            decimal discountValue
            );

        Task<Order?> GetOrderByIdAsync(int id);
    }
}

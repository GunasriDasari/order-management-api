using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Application.Discounts;
using Microsoft.Extensions.Logging;

namespace OrderManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDiscountStrategyFactory _discountStrategyFactory;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository, IDiscountStrategyFactory discountStrategyFactory, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _discountStrategyFactory = discountStrategyFactory;
            _logger = logger;

        }

        public async Task<Order> CreateOrderAsync(
            int customerId, 
            Dictionary<int, int> productQuantities,
            string? discountType,
            decimal discountValue)
        {
            _logger.LogInformation("Creating order for CustomerId: {CustomerId}", customerId);

            var customer = await _customerRepository.GetByIdAsync(customerId);

            if(customer == null)
            {
                _logger.LogWarning("Customer not found. CustomerId: {CustomerId}", customerId);

                throw new KeyNotFoundException("Customer not found.");
            }

            if(productQuantities == null || !productQuantities.Any())
            {
                throw new ArgumentException("Order must contain at least one product.");
            }

            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach(var item in productQuantities)
            {
                var productId = item.Key;
                var quantity = item.Value;

                var product = await _productRepository.GetByIdAsync(productId);

                if (product == null)
                {
                    _logger.LogWarning("Product not found. ProductId: {ProductId}", productId);
                    throw new KeyNotFoundException($"Product with id {productId} not found.");
                }

                if (quantity <= 0)
                {
                    throw new ArgumentException("Quantity must be greater than zero.");
                }

                if (product.StockQuantity < quantity)
                {
                    throw new InvalidOperationException($"Not enough stock for product {product.Name}.");
                }

                var subTotal = product.Price * quantity;

                var orderItem = new OrderItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    SubTotal = subTotal
                };

                order.OrderItems.Add(orderItem);

                totalAmount += subTotal;
            }

            var discountStrategy = _discountStrategyFactory.GetStrategy(discountType, discountValue);

            var finalAmount = discountStrategy.ApplyDiscount(totalAmount);

            order.TotalAmount = finalAmount;

            _logger.LogInformation(
                "Order total calculated. OriginalAmount: {OriginalAmount}, FinalAmount: {FinalAmount}, DiscountType: {DiscountType}",
                totalAmount,
                finalAmount,
                discountType);

            var createdOrder = await _orderRepository.CreateAsync(order);

            _logger.LogInformation("Order created successfully. OrderId: {OrderId}", createdOrder.Id);

            return createdOrder;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }
    }
}

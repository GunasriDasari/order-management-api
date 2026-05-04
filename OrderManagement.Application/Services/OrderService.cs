using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Application.Discounts;

namespace OrderManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDiscountStrategyFactory _discountStrategyFactory;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository, IDiscountStrategyFactory discountStrategyFactory)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _discountStrategyFactory = discountStrategyFactory;
        }

        public async Task<Order> CreateOrderAsync(
            int customerId, 
            Dictionary<int, int> productQuantities,
            string? discountType,
            decimal discountValue)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if(customer == null)
            {
                throw new Exception("Customer not Found");
            }

            if(productQuantities == null || !productQuantities.Any())
            {
                throw new Exception("Order must contain atleast one product");
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

                if(product == null)
                {
                    throw new Exception($"Product with id {productId} not found");
                }

                if(quantity <= 0)
                {
                    throw new Exception("Quantiry must be greater than zero");
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

            return await _orderRepository.CreateAsync(order);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }
    }
}

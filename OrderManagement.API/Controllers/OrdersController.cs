using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            _logger.LogInformation("Create order request received for CustomerId: {CustomerId}", request.CustomerId);

            var order = await _orderService.CreateOrderAsync(
                request.CustomerId,
                request.ProductQuantities,
                request.DiscountType,
                request.DiscountValue
                );
            var response = MapToOrderResponse(order);

            return CreatedAtAction(nameof(GetOrderById), new { id = response.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            _logger.LogInformation("Get order request received for OrderId: {OrderId}", id);

            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                _logger.LogWarning("Order not found. OrderId: {OrderId}", id);
                return NotFound();
            }

            var response = MapToOrderResponse(order);

            return Ok(response);
        }

        private static OrderResponse MapToOrderResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(item => new OrderItemResponse
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? string.Empty,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    SubTotal = item.SubTotal
                }).ToList()
            };
        }
    }
}
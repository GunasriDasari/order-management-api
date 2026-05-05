using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Services;
using OrderManagement.API.Models;

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
            return CreatedAtAction(nameof(GetOrderById), new { id =  order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            _logger.LogInformation("Get order request received for OrderId: {OrderId}", id);

            var order = await _orderService.GetOrderByIdAsync(id);

            if(order == null)
            {
                _logger.LogWarning("Order not found. OrderId: {OrderId}", id);

                return NotFound();
            }

            return Ok(order);
        }
    }
}

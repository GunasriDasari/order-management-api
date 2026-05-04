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

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
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
            var order = await _orderService.GetOrderByIdAsync(id);

            if(order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}

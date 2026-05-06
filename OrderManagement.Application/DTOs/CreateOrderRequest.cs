using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.DTOs
{
    public class CreateOrderRequest
    {

        [Required]
        [MinLength(1, ErrorMessage = "Order must contain at least one product.")]
        public Dictionary<int, int> ProductQuantities { get; set; } = new();

        public string? DiscountType { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountValue { get; set; }
    }
}
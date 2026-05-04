namespace OrderManagement.API.Models
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }

        public Dictionary<int, int> ProductQuantities { get; set; } = new();

        public string? DiscountType { get; set; }

        public decimal DiscountValue { get; set; }
    }
}
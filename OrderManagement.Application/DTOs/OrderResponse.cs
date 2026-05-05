namespace OrderManagement.Application.DTOs
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public List<OrderItemResponse> Items { get; set; } = new();
    }
}
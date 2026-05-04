
namespace OrderManagement.Application.Discounts
{
    public class PercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly decimal _percentage;

        public PercentageDiscountStrategy(decimal percentage)
        {
            _percentage = percentage;
        }

        public decimal ApplyDiscount(decimal totalAmount)
        {
            var discountAmount = totalAmount * (_percentage / 100);
            return totalAmount - discountAmount;
        }
    }
}

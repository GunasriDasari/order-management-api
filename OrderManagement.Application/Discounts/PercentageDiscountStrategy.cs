namespace OrderManagement.Application.Discounts
{
    public class PercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly decimal _percentage;

        public PercentageDiscountStrategy(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentException("Discount percentage must be between 0 and 100.");
            }

            _percentage = percentage;
        }

        public decimal ApplyDiscount(decimal totalAmount)
        {
            if (totalAmount < 0)
            {
                throw new ArgumentException("Total amount cannot be negative.");
            }

            var discountAmount = totalAmount * (_percentage / 100);
            return totalAmount - discountAmount;
        }
    }
}
namespace OrderManagement.Application.Discounts
{
    public class DiscountStrategyFactory : IDiscountStrategyFactory
    {
        public IDiscountStrategy GetStrategy(string? discountType, decimal discountValue)
        {
            if (string.IsNullOrWhiteSpace(discountType))
            {
                return new NoDiscountStrategy();
            }

            return discountType.ToLower() switch
            {
                "percentage" => new PercentageDiscountStrategy(discountValue),
                "none" => new NoDiscountStrategy(),
                _ => throw new ArgumentException("Invalid discount type.")
            };
        }
    }
}


namespace OrderManagement.Application.Discounts
{
    public interface IDiscountStrategyFactory
    {
        IDiscountStrategy GetStrategy(string? discountType, decimal discountValue);
    }
}

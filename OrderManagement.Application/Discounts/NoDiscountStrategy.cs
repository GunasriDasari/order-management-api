

namespace OrderManagement.Application.Discounts
{
    public class NoDiscountStrategy : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal totalAmount)
        {
            return totalAmount;
        }
    }
}

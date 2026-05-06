using OrderManagement.Application.Discounts;

namespace OrderManagement.Tests.Discounts
{
    public class NoDiscountStrategyTests
    {
        [Fact] // this tells xUnit that this is a test method
        public void ApplyDiscount_ShouldReturnSameAmount_WhenNoDiscountIsApplied()
        {
            //Arrange - used to prepare data
            var strategy = new NoDiscountStrategy();
            decimal totalAmount = 100m;

            //Act - this will call the method
            var result = strategy.ApplyDiscount(totalAmount);

            //Assert - used to check the result
            Assert.Equal(100m, result);
        }
    }
}

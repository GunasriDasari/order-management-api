using OrderManagement.Application.Discounts;

namespace OrderManagement.Tests.Discounts
{
    public class PercentageDiscountStrategyTests
    {
        [Fact]
        public void ApplyDiscount_ShouldReturnDiscountedAmount()
        {
            //Arrange 
            var strategy = new PercentageDiscountStrategy(10);
            decimal totalAmount = 200m;

            //Act
            var result = strategy.ApplyDiscount(totalAmount);

            //Assert
            Assert.Equal(180m, result);  
        }


        [Fact]
        public void Constructor_ShouldThrowArgumentedException_WhenPercentageIsGreaterThan100()
        {
            decimal invalidPercentage = 120m;

            Assert.Throws<ArgumentException>(() => new PercentageDiscountStrategy(invalidPercentage));
        }
    }

}
     
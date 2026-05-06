using Microsoft.Extensions.Logging;
using Moq;
using OrderManagement.Application.Discounts;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IDiscountStrategyFactory> _discountFactoryMock;
        private readonly Mock<ILogger<OrderService>> _loggerMock;

        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _discountFactoryMock = new Mock<IDiscountStrategyFactory>();
            _loggerMock = new Mock<ILogger<OrderService>>();

            _orderService = new OrderService(
                _orderRepositoryMock.Object,
                _productRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _discountFactoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrderSuccessfully()
        {
            //Arrange
            var customer = new Customer
            {
                Id = 1,
                Name = "Guna",
                Email = "guna@gmail.com"
            };

            var product = new Product
            {
                Id = 1,
                Name = "Keyboard",
                Price = 100,
                StockQuantity = 10
            };

            var productQuantities = new Dictionary<int, int>
            {
                { 1,2 }
            };

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(customer);

            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            _discountFactoryMock
                .Setup(x => x.GetStrategy(null,0))
                .Returns(new NoDiscountStrategy());

            _orderRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order order) => order);

            //Act
            var result = await _orderService.CreateOrderAsync(
                1,
                productQuantities,
                null,
                0);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.TotalAmount);
            Assert.Single(result.OrderItems);
            Assert.Equal(2, result.OrderItems.First().Quantity);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowKeyNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var productQuantities = new Dictionary<int, int>
            {
                { 1,2  }
            };

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Customer?)null);

            // Act & Assert

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _orderService.CreateOrderAsync(
                99,
                productQuantities,
                null,
                0));
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            var customer = new Customer
            {
                Id = 1,
                Name = "Test",
                Email = "test@email.com",
            };

            var productQuantities = new Dictionary<int, int> 
            { 
                { 1,2 } 
            };

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(customer);

            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _orderService.CreateOrderAsync(
                1,
                productQuantities,
                null,
                0));
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowArgumentException_WhenQuantityIsInvalid()
        {
            //Arrange

            var customer = new Customer
            {
                Id = 1,
                Name = "Test",
                Email = "test1@gmail.com"
            };

            var product = new Product
            {
                Id = 1,
                Name = "Keyboard",
                Price = 100,
                StockQuantity = 10
            };

            var productQuantities = new Dictionary<int, int>
            {
                { 1,0 }
            };

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(customer);

            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            //Assert

            await Assert.ThrowsAsync<ArgumentException>(() =>
            _orderService.CreateOrderAsync(1, productQuantities, null, 0));
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowArgumentException_WhenProductQuantitiesIsEmpty()
        {
            var customer = new Customer
            {
                Id = 1,
                Name = "Test",
                Email = "test@gmail.com"
            };

            var productQuantities = new Dictionary<int, int>();

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(customer);

            await Assert.ThrowsAsync<ArgumentException>(() =>
            _orderService.CreateOrderAsync(1, productQuantities, null, 0));
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowInvalidOperationException_WhenStockIsInsufficient()
        {
            var customer = new Customer
            {
                Id = 1,
                Name = "Test",
                Email = "test@gmail.com"
            };

            var product = new Product
            {
                Id = 1,
                Name = "KeyBoard",
                Price = 100,
                StockQuantity = 2
            };

            var productQuantities = new Dictionary<int, int>()
            {
                {1,5 }
            };

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(customer);

            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _orderService.CreateOrderAsync(1, productQuantities, null, 0));
        }
    }
}

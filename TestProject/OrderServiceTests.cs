using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Moq;
using Services;
using ServicesAbstraction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class OrderServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
        private readonly Mock<IPaymentStrategyFactory> _mockPaymentFactory = new();
        private readonly Mock<IEmailService> _mockEmailService = new();
        private readonly IMapper _mapper;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderResponseDto>();
                cfg.CreateMap<ProductDto, Product>();
                cfg.CreateMap<Product, ProductDto>();
            });

            _mapper = config.CreateMapper();

            _orderService = new OrderService(
                  _mockUnitOfWork.Object,
                  _mockEmailService.Object,            
                  _mockPaymentFactory.Object,          
                  _mapper
            );
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrow_WhenCustomerNotFound()
        {
            _mockUnitOfWork.Setup(u => u.CustomerRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Customer)null);

            var dto = new OrderRequestDto { CustomerId = 1, Items = new List<OrderItemDetailDto>() };

            await Assert.ThrowsAsync<NotFoundException>(() => _orderService.CreateOrderAsync(dto));
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrow_WhenPaymentFails()
        {
            _mockUnitOfWork.Setup(u => u.CustomerRepository.GetByIdAsync(1)).ReturnsAsync(new Customer());
            _mockUnitOfWork.Setup(u => u.ProductRepository.GetByIdAsync(1)).ReturnsAsync(new Product { Stock = 5, Price = 100 });
            _mockPaymentFactory.Setup(f => f.GetStrategy(It.IsAny<string>())).Returns(new Mock<IPaymentService>().Object);

            var dto = new OrderRequestDto
            {
                CustomerId = 1,
                PaymentMethod = "creditcard",
                Items = new List<OrderItemDetailDto> { new OrderItemDetailDto { ProductId = 1, Quantity = 1 } }
            };

            await Assert.ThrowsAsync<AppException>(() => _orderService.CreateOrderAsync(dto));
        }
        
        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrder_WhenExists()
        {
            _mockUnitOfWork.Setup(u => u.OrderRepository.GetByIdAsync(1)).ReturnsAsync(new Order { OrderId = 1 });

            var result = await _orderService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.OrderId);
        }
    }

}

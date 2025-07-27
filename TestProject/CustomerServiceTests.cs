using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Moq;
using Services;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class CustomerServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
        private readonly IMapper _mapper;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<ProductDto, Product>();
            });
            _mapper = config.CreateMapper();

            _customerService = new CustomerService(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenEmailIsMissing()
        {
            var dto = new CustomerDto { Name = "Test User", Email = "" };
            await Assert.ThrowsAsync<ArgumentException>(() => _customerService.AddAsync(dto));
        }

        [Fact]
        public async Task GetOrdersAsync_ShouldReturnCustomerOrders()
        {
            var orders = new List<Order>
        {
            new Order { CustomerId = 1, OrderId = 1 },
            new Order { CustomerId = 2, OrderId = 2 },
        };

            _mockUnitOfWork.Setup(x => x.CustomerRepository.GetByIdAsync(1)).ReturnsAsync(new Customer { CustomerId = 1 });
            _mockUnitOfWork.Setup(x => x.OrderRepository.GetAllAsync()).ReturnsAsync(orders);

            var result = await _customerService.GetOrdersAsync(1);
            Assert.Single(result);
        }
    }

}

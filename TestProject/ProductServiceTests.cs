using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Moq;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
        private readonly IMapper _mapper;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<ProductDto, Product>();
            });

            _mapper = config.CreateMapper();
            _productService = new ProductService(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProducts()
        {
            var products = new List<Product> { new Product { ProductId = 1, Name = "Test", Stock = 10, Price = 50 } };
            _mockUnitOfWork.Setup(x => x.ProductRepository.GetAllAsync()).ReturnsAsync(products);

            var result = await _productService.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("Test", result.First().Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct()
        {
            _mockUnitOfWork.Setup(x => x.ProductRepository.GetByIdAsync(1)).ReturnsAsync(new Product { ProductId = 1, Name = "Sample" });
            var result = await _productService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Sample", result.Name);
        }
    }
}

using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
        private readonly IMapper _mapper;
        private readonly InvoiceService _invoiceService;

        public InvoiceServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Invoice, InvoiceDto>();
            });
            _mapper = config.CreateMapper();

            _invoiceService = new InvoiceService(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenInvoiceNotFound()
        {
            _mockUnitOfWork.Setup(x => x.InvoiceRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Invoice)null);
            await Assert.ThrowsAsync<NotFoundException>(() => _invoiceService.GetByIdAsync(1));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnInvoices()
        {
            _mockUnitOfWork.Setup(x => x.InvoiceRepository.GetAllAsync()).ReturnsAsync(new List<Invoice> { new Invoice { InvoiceId = 1 } });
            var result = await _invoiceService.GetAllAsync();

            Assert.Single(result);
            Assert.Equal(1, result.First().InvoiceId);
        }
    }

}

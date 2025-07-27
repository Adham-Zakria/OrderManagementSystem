using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderManagementDbContext _dbContext;

        private readonly Lazy<ICustomerRepository> _customerRepository;
        private readonly Lazy<IInvoiceRepository> _invoiceRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<IUserRepository> _userRepository;

        public UnitOfWork(OrderManagementDbContext dbContext)
        {
            _dbContext = dbContext;

            _customerRepository = new Lazy<ICustomerRepository>(() => new CustomerRepository(_dbContext));
            _invoiceRepository = new Lazy<IInvoiceRepository>(() => new InvoiceRepository(_dbContext));
            _orderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(_dbContext));
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(_dbContext));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_dbContext));
        }

        public ICustomerRepository CustomerRepository => _customerRepository.Value;
        public IInvoiceRepository InvoiceRepository => _invoiceRepository.Value;
        public IOrderRepository OrderRepository => _orderRepository.Value;
        public IProductRepository ProductRepository => _productRepository.Value;
        public IUserRepository UserRepository => _userRepository.Value;

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

    }
}

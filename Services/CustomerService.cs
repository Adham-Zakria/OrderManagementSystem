using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using ServicesAbstraction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CustomerService(IUnitOfWork _unitOfWork, IMapper _mapper) : ICustomerService
    {
        public async Task<int> AddAsync(CustomerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Customer name and email are required.");

            var entity = _mapper.Map<Customer>(dto);
            await _unitOfWork.CustomerRepository.AddAsync(entity);
            return _unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<OrderResponseDto>> GetOrdersAsync(int customerId)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);

            if (customer == null)
                throw new NotFoundException("customer not found");

            var orders = await _unitOfWork.OrderRepository.GetAllAsync();
            var customerOrders = orders.Where(o => o.CustomerId == customerId);
            return _mapper.Map<IEnumerable<OrderResponseDto>>(customerOrders);
        }
    }
}

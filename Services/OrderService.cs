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
    public class OrderService (
            IUnitOfWork _unitOfWork,
            IEmailService _emailService,
            IPaymentStrategyFactory _paymentStrategyFactory,
            IMapper _mapper) : IOrderService
    {
        
        public async Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto request)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
                throw new NotFoundException("Customer not found.");

            var orderItems = new List<OrderItem>();

            foreach (var itemDto in request.Items)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null || product.Stock < itemDto.Quantity)
                    throw new AppException($"Insufficient stock for product {product?.Name ?? "N/A"}");

                product.Stock -= itemDto.Quantity;
                await _unitOfWork.ProductRepository.UpdateAsync(product);

                orderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price,
                    Discount = 0
                });
            }

            decimal total = orderItems.Sum(i => i.Quantity * i.UnitPrice);
            if (total > 200)
                total *= 0.90m;
            else if (total > 100)
                total *= 0.95m;

            // Process payment using selected method
            var paymentStrategy = _paymentStrategyFactory.GetStrategy(request.PaymentMethod.ToLower());
            var paymentSucceeded = await paymentStrategy.ProcessPaymentAsync(total);

            if (!paymentSucceeded)
                throw new AppException("Payment failed. Order was not placed.");

            // Proceed only if payment succeeded
            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderItems = orderItems,
                TotalAmount = total,
                PaymentMethod = request.PaymentMethod,
                Status = "Pending",
                OrderDate = DateTime.UtcNow
            };

            await _unitOfWork.OrderRepository.AddAsync(order);

            // Generate invoice
            var invoice = new Invoice
            {
                OrderId = order.OrderId,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = total
            };
            await _unitOfWork.InvoiceRepository.AddAsync(invoice);

            // Send email notification
            await _emailService.SendOrderStatusChangedEmailAsync(customer.Email, order.Status);

            var fullOrder = await _unitOfWork.OrderRepository.GetByIdAsync(order.OrderId);
            return _mapper.Map<OrderResponseDto>(fullOrder);
        }

        public async Task<OrderResponseDto> GetByIdAsync(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null)
                throw new NotFoundException("Order not found");

            return _mapper.Map<OrderResponseDto>(order);
        }


        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }


        public async Task UpdateStatusAsync(int orderId, string status)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException("Order not found");

            order.Status = status;
            await _unitOfWork.OrderRepository.UpdateStatusAsync(orderId, status);

            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(order.CustomerId);
            if (customer != null)
            {
                await _emailService.SendOrderStatusChangedEmailAsync(customer.Email, status);
            }
        }
    }
}

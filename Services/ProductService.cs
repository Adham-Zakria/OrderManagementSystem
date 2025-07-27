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
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) throw new NotFoundException("Product not found");

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<int> AddAsync(ProductDto product)
        {
            if (product.Stock < 0 || product.Price < 0)
                throw new ArgumentException("Price and stock must be non-negative.");

            var entity = _mapper.Map<Product>(product);
            await _unitOfWork.ProductRepository.AddAsync(entity);
            return _unitOfWork.SaveChanges();
        }

        public async Task<int> UpdateAsync(int id, ProductDto dto)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            if (product == null) 
                throw new NotFoundException("Product not found");
            if (dto.Stock < 0 || dto.Price < 0)
                throw new ArgumentException("Price and stock must be non-negative.");

            var entity = _mapper.Map<Product>(dto);
            await _unitOfWork.ProductRepository.UpdateAsync(entity);
            return _unitOfWork.SaveChanges();
        }
    }
}

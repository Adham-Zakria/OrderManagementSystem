using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using ServicesAbstraction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class InvoiceService(IUnitOfWork _unitOfWork, IMapper _mapper) : IInvoiceService
    {
        public async Task<IEnumerable<InvoiceDto>> GetAllAsync()
        {
            var invoices = await _unitOfWork.InvoiceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }

        public async Task<InvoiceDto> GetByIdAsync(int id)
        {
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id);
            if (invoice == null) throw new NotFoundException("Invoice not found");

            return _mapper.Map<InvoiceDto>(invoice);
        }
    }
}

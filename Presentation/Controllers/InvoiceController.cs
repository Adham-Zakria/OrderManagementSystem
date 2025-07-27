using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController(IInvoiceService _invoiceService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAll()
        {
            var invoices = await _invoiceService.GetAllAsync();
            return Ok(invoices);
        }

        [HttpGet("{invoiceId}")]
        public async Task<ActionResult<InvoiceDto>> GetById(int invoiceId)
        {
            var invoice = await _invoiceService.GetByIdAsync(invoiceId);
            return Ok(invoice);
        }
    }
}

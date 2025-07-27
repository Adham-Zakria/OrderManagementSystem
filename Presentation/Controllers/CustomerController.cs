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
    public class CustomerController(ICustomerService _customerService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddCustomer([FromBody] CustomerDto dto)
        {
            var id = await _customerService.AddAsync(dto);
            return Ok(id);
        }

        [HttpGet("{customerId}/orders")]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetCustomerOrders(int customerId)
        {
            var orders = await _customerService.GetOrdersAsync(customerId);
            return Ok(orders);
        }
    }
}

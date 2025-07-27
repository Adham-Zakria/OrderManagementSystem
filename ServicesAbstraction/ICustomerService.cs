using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
    public interface ICustomerService
    {
        Task<int> AddAsync(CustomerDto dto);
        Task<IEnumerable<OrderResponseDto>> GetOrdersAsync(int customerId);
    }
}

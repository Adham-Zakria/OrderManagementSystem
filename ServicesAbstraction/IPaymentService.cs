using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(decimal amount, string currency = "USD");
    }
}

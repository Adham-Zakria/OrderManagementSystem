using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
    public interface IEmailService
    {
        Task SendOrderStatusChangedEmailAsync(string customerEmail, string orderStatus);
    }
}

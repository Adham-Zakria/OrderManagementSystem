using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class OrderRequestDto
    {
        public int CustomerId { get; set; }
        public string PaymentMethod { get; set; } 
        public List<OrderItemDetailDto> Items { get; set; } = new();
    }
}

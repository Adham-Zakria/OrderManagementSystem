using Microsoft.Extensions.DependencyInjection;
using ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PaymentStrategyFactory(IServiceProvider _provider) : IPaymentStrategyFactory
    {
        public IPaymentService GetStrategy(string method)
        {
            return method.ToLower() switch
            {
                "paypal" => _provider.GetRequiredService<PayPalPaymentStrategy>(),
                "creditcard" => _provider.GetRequiredService<CreditCardPaymentStrategy>(),
                _ => throw new ArgumentException("Unsupported payment method")
            };
        }
    }
}

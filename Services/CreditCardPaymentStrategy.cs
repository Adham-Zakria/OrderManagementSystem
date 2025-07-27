using Microsoft.Extensions.Options;
using Services.Helper;
using ServicesAbstraction;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CreditCardPaymentStrategy : IPaymentService
    {
        public CreditCardPaymentStrategy(IOptions<StripeSettings> options)
        {
            StripeConfiguration.ApiKey = options.Value.SecretKey;
        }

        public async Task<bool> ProcessPaymentAsync(decimal amount, string currency = "usd")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // Convert to cents
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" },
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            // Adjust this depending on how your frontend handles Stripe client_secret
            return paymentIntent.Status == "requires_payment_method";
        }
    }
}

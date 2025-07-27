using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Services.Helper;
using ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PayPalPaymentStrategy : IPaymentService
    {
        private readonly PayPalHttpClient _client;

        public PayPalPaymentStrategy(IOptions<PayPalSettings> options)
        {
            var settings = options.Value;

            var environment = new SandboxEnvironment(settings.ClientId, settings.ClientSecret);
            _client = new PayPalHttpClient(environment);
        }

        public async Task<bool> ProcessPaymentAsync(decimal amount, string currency = "USD")
        {
            var orderRequest = new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    AmountWithBreakdown = new AmountWithBreakdown
                    {
                        CurrencyCode = currency,
                        Value = amount.ToString("F2")
                    }
                }
            }
            };

            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(orderRequest);

            var response = await _client.Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.Created;
        }
    }
}

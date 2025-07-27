using Domain.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Services.Helper;
using Services.MappingProfiles;
using ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ApplicationServiceRegisteration
    {
        public static IServiceCollection AddApplicationService
            (this IServiceCollection services, IConfiguration configuration)
        {


            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IUserService, UserService>();

            services.AddAutoMapper(config=>
            {
                config.AddProfile<MappingProfile>();
            });

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, EmailService>();


            // Add JWT authentication
            var jwtSettings = configuration.GetSection("Jwt");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings["Key"])
                        )
                    };
                });

            // Add Payment services
            services.Configure<PayPalSettings>(configuration.GetSection("PayPal"));
            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));

            services.AddTransient<PayPalPaymentStrategy>();
            services.AddTransient<CreditCardPaymentStrategy>();
            services.AddScoped<IPaymentStrategyFactory, PaymentStrategyFactory>();


            return services;

        }
    }
}

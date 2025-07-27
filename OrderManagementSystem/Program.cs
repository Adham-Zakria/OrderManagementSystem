
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementSystem.Middlewares;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Presentation.Controllers;
using Services;
using Services.Helper;
using Services.MappingProfiles;
using ServicesAbstraction;


namespace OrderManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region Services' Container

            //builder.Services.AddControllers();
            builder.Services.AddControllers().AddApplicationPart(typeof(UserController).Assembly);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<OrderManagementDbContext>(options =>
                  options.UseInMemoryDatabase("OrderManagementDb"));

            builder.Services.AddInfrastructureRegisteration(builder.Configuration);
            builder.Services.AddApplicationService(builder.Configuration);

            #endregion

            var app = builder.Build();

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

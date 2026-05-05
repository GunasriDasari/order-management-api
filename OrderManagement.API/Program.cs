using Microsoft.EntityFrameworkCore;
using OrderManagement.Infrastructure.Data;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using OrderManagement.Infrastructure.Repositories;
using OrderManagement.Application.Discounts;
using OrderManagement.API.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository> ();
builder.Services.AddScoped<IProductRepository, ProductRepository> ();
builder.Services.AddScoped<IOrderRepository, OrderRepository> ();

builder.Services.AddScoped<IOrderService, OrderService> ();
builder.Services.AddScoped<IDiscountStrategyFactory, DiscountStrategyFactory> ();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
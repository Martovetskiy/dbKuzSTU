using Lab1.Application.Interfaces;
using Lab1.Application.Services;
using Lab1.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICustomerRepository>(new CustomerRepository(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddSingleton<CustomerService>();

builder.Services.AddSingleton<ICarRepository>(new CarRepository(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddSingleton<CarService>();

builder.Services.AddSingleton<IRentalRepository>(new RentalRepository(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddSingleton<RentalService>();

builder.Services.AddSingleton<IPaymentRepository>(new PaymentRepository(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddSingleton<PaymentService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddServiceDefaults();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapControllers();
app.MapDefaultEndpoints();

app.MapGet("/health", () => "OK");

app.Run();
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.API.EventBusConsumer;
using Ordering.Application;
using Ordering.Infrastructure.DI;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var orderContext = builder.Services.BuildServiceProvider().GetService<OrderContext>();
orderContext.Database.Migrate();

var logger = builder.Services.BuildServiceProvider().GetService<ILogger<OrderContextSeed>>();
OrderContextSeed.SeedAsync(orderContext, logger);


builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        configuration.ReceiveEndpoint(EventBaseConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(context);
        });
    });
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<BasketCheckoutConsumer>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.MigrateDatabase<OrderContext>((context, services) =>
//{
//    var logger = services.GetService<ILogger<OrderContextSeed>>();
//    OrderContextSeed
//        .SeedAsync(context, logger)
//        .Wait();
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
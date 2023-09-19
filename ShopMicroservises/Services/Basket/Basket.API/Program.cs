using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
                        (o => o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!));
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
builder.Services.AddOptions<MassTransitHostOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

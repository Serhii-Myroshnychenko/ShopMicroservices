using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOcelot();

builder.Host
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
    })
    .ConfigureLogging((loggingContext, loggingBuilder) =>
    {
        loggingBuilder.AddConfiguration(loggingContext.Configuration.GetSection("Logging"));
        loggingBuilder.AddConsole();
        loggingBuilder.AddDebug();
    });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
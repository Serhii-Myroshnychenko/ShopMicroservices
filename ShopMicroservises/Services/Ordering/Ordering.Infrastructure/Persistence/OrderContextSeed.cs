using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;


namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
        {
            new Order()
            {
                UserName = "JohnDoe",
                TotalPrice = 100.00M,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "johndoe@example.com",
                AddressLine = "123 Main St",
                Country = "USA",
                State = "CA",
                ZipCode = "12345",
                CardName = "John Doe",
                CardNumber = "1234-5678-9876-5432", // This is just a sample, use a valid card number
                Expiration = "12/24",              // Sample expiration date (MM/YY)
                CVV = "123",                       // Sample CVV
                PaymentMethod = 1,
                CreatedBy = "JohnDoe",
                CreatedDate = DateTime.UtcNow,
                LastModifiedBy = "JohnDoe",
                LastModifiedDate = DateTime.UtcNow
            }
        };
    }
}

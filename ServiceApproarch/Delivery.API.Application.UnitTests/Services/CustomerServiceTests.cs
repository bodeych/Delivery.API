using Delivery.API.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace Delivery.API.Application.UnitTests.Services;

public class CustomerServiceTests
{
    [Fact]
    public async Task FindById_ValidUserId_ReturnListOrders()
    {
        // Arrange
        var fakeCustomerRepository = new FakeCustomerRepository();
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var customerService = new CustomerService(fakeCustomerRepository, memoryCache);
         
        var userId = new Guid("8d220771-aa54-4164-a361-2cc3c292fee5");
        
        var pickup = Coordinate.Create(12.0d, 11.0d);
        var dropoff = Coordinate.Create(10.0d, 9.0d);
        var distance = Distance.FromMeters(200);
        var cost = 23.1m;
        var order = Order.Create(userId, pickup, dropoff, distance, cost);
        fakeCustomerRepository.Orders.Add(order);
        
        var newPickup = Coordinate.Create(8.0d, 8.0d);
        var newDropoff = Coordinate.Create(8.0d, 8.0d);
        var newDistance = Distance.FromMeters(1200);
        var newCost = 10.1m;
        var newOrder = Order.Create(userId, newPickup, newDropoff, newDistance, newCost);
        fakeCustomerRepository.Orders.Add(order);
        
        // Act
        var listOrders = await customerService.FindById(userId, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(listOrders);
            Assert.Equal(2, listOrders.Count);
        });
    }
    
    [Fact]
    public async Task FindById_invalidUserId_ReturnNull()
    {
        // Arrange
        var fakeCustomerRepository = new FakeCustomerRepository();
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var customerService = new CustomerService(fakeCustomerRepository, memoryCache);

        var userId = Guid.NewGuid();
        
        // Act
        var listOrders = await customerService.FindById(userId, CancellationToken.None);

        // Assert
        Assert.Null(listOrders);
    }
}
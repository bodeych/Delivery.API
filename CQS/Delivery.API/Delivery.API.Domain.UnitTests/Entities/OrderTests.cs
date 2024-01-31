namespace Delivery.API.Domain.UnitTests.Entities;

public class OrderTests
{
    [Fact]
    public async Task Create_CreateOrder()
    {
        // Arrange
        var userId = new Guid("8d220771-aa54-4164-a361-2cc3c292fee5");
        var pickup = Coordinate.Create(12.0d, 11.0d);
        var dropoff = Coordinate.Create(10.0d, 9.0d);
        var distance = Distance.FromMeters(200);
        var cost = 23.1m;
        // Act
        var order = Order.Create(userId, pickup, dropoff, distance, cost);

        // Assert
        Assert.Equal(userId, order.UserId);
        Assert.Equal(pickup, order.Pickup);
        Assert.Equal(dropoff, order.Dropoff);
        Assert.Equal(distance, order.Distance);
        Assert.Equal(cost, order.Cost);
    }
}
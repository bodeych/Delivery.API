namespace Delivery.API.Domain.UnitTests.Entities;

public class OrderTests
{
    [Fact]
    public async Task Create_CoefIsNull_CreateOrder()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var pickup = Coordinate.Create(12.0, 11.0);
        var dropoff = Coordinate.Create(10.0, 9.0);
        float? distanceCoef = null;
        // Act
        var order = Order.Create(userId, pickup, dropoff, distanceCoef);

        // Assert
        using var scope = new AssertionScope();
        order.UserId.Should().Be(userId);
        order.Pickup.Should().Be(pickup);
        order.Dropoff.Should().Be(dropoff);
        order.Distance.Should().NotBeNull();
        order.Cost.Should().BePositive();
    }
    
    [Fact]
    public async Task Create_SetCoef_CreateOrder()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var pickup = Coordinate.Create(12.0, 11.0);
        var dropoff = Coordinate.Create(10.0, 9.0);
        float? distanceCoef = 1.8f;
        // Act
        var order = Order.Create(userId, pickup, dropoff, distanceCoef);

        // Assert
        using var scope = new AssertionScope();
        order.UserId.Should().Be(userId);
        order.Pickup.Should().Be(pickup);
        order.Dropoff.Should().Be(dropoff);
        order.Distance.Should().NotBeNull();
        order.Cost.Should().BePositive();
    }
}
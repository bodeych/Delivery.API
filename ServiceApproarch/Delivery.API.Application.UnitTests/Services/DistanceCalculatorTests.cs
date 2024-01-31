using Delivery.API.Application.Services;
using Delivery.API.Domain.ValueObjects;

namespace Delivery.API.Application.UnitTests.Services;

public class DistanceCalculatorTests
{
    [Fact]
    public async Task CalculateDistanceMeters_CalculateDistanceMeters()
    {
        // Arrange
        var distance = new DistanceCalculator();
        var pickup = Coordinate.Create(52.332576, 14.529904);
        var dropoff = Coordinate.Create(52.325721, 14.516244);
        
        // Act
        var result = distance.CalculateDistanceMeters(pickup, dropoff);
        

        // Assert
        Assert.Equal(1201, result);  //??
    }
}
namespace Delivery.API.Domain.UnitTests.ValueObjects;

public class DistanceTests
{
    [Fact]
    public async Task FromMeters_CreateDistanceFromMeters()
    {
        // Arrange
        var meters = 100;
        
        // Act
        var distance = Distance.FromMeters(meters);
        
        // Assert
        distance.Meters.Should().Be(meters);
    }
    
    [Fact]
    public async Task FromMeters_ReturnArgumentException()
    {
        // Arrange
        var meters = -200;
        
        // Act
        var test = () => Distance.FromMeters(meters);
        
        //  Assert
        test.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public async Task FromKilometers_CreateDistanceFromKilometers()
    {
        // Arrange
        var kilometers = 2;
        
        // Act
        var distance = Distance.FromKilometers(kilometers);
        
        // Assert
        distance.Kilometers.Should().Be(kilometers);
    }
    
    [Fact]
    public async Task FromKilometers_ThrowArgumentException()
    {
        // Arrange
        var kilometers = -1;
        
        // Act
        var test = () => Distance.FromKilometers(kilometers);
        
        //  Assert
        test.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public async Task CreateFromToPoints_ValidData_ReturnResult()
    {
        // Arrange
        var pickup = Coordinate.Create(52.332576, 14.529904);
        var dropoff = Coordinate.Create(52.325721, 14.516244);
        
        // Act
        var distance = Distance.CreateFromToPoints(pickup, dropoff);
        
        //  Assert
        distance.Meters.Should().Be(1201);
    }
}
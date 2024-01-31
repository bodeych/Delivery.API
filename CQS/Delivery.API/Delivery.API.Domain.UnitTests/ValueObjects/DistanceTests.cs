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
        Assert.Equal(meters, distance.Meters);
    }
    
    [Fact]
    public async Task FromMeters_ReturnArgumentException()
    {
        // Arrange
        var meters = -200;
        
        // ???
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Distance.FromMeters(meters));
    }
    
    [Fact]
    public async Task FromKilometers_CreateDistanceFromKilometers()
    {
        // Arrange
        var kilometers = 2;
        
        // Act
        var distance = Distance.FromKilometers(kilometers);
        
        // Assert
        Assert.Equal(kilometers, distance.Kilometers);
    }
    
    [Fact]
    public async Task FromKilometers_ThrowArgumentException()
    {
        // Arrange
        var kilometers = -1;
        
        // ???
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Distance.FromKilometers(kilometers));
    }
}
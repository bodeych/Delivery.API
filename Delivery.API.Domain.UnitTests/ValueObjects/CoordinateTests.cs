namespace Delivery.API.Domain.UnitTests.ValueObjects;

public class CoordinateTests
{
    [Fact]
    public async Task Create_CreateCoordinate()
    {
        // Arrange
        var latitude = 1.1d;
        var longitude = 2.2d;
        
        // Act
        var coordinate = Coordinate.Create(latitude, longitude);
        
        // Assert
        Assert.Equal(latitude, coordinate.Latitude);
        Assert.Equal(longitude, coordinate.Longitude);
    }
    
    [Fact]
    public async Task Create_ThrowArgumentExceptionOfLatitude()
    {
        // Arrange
        var latitude = -91d;
        var longitude = 2.2d;
        
        // ????
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Coordinate.Create(latitude, longitude));
    }
    
    [Fact]
    public async Task Create_ThrowArgumentExceptionOfLongitude()
    {
        // Arrange
        var latitude = 2.2d;
        var longitude = 181d;
        
        // ????
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Coordinate.Create(latitude, longitude));
    }
}
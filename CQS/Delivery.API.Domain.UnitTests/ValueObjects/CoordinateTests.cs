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
        using var scope = new AssertionScope();
        coordinate.Latitude.Should().Be(latitude);
        coordinate.Longitude.Should().Be(longitude);
    }
    
    [Fact]
    public async Task Create_ThrowArgumentExceptionOfLatitude()
    {
        // Arrange
        var latitude = -91d;
        var longitude = 2.2d;
        
        // Act
        var test = () => Coordinate.Create(latitude, longitude);
        
        // Assert
        test.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public async Task Create_ThrowArgumentExceptionOfLongitude()
    {
        // Arrange
        var latitude = 2.2d;
        var longitude = 181d;
        
        // Act
        var test = () => Coordinate.Create(latitude, longitude);
        
        // Assert
        test.Should().Throw<ArgumentException>();
    }
}
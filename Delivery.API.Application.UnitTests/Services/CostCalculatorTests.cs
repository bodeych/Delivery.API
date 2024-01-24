using Delivery.API.Application.Services;
using Delivery.API.Application.Settings;
using Delivery.API.Domain.ValueObjects;

namespace Delivery.API.Application.UnitTests.Services;

public class CostCalculatorTests
{
    
    [Fact]
    public async Task Calculate_WithSettingsCoef_CalculateDistanceCostPerKm()
    {
        // Arrange
        var settings = new OrderSettings
        {
            CostPerKm = 1.8f
        };
        var costCalculator = new CostCalculator(settings); 
        var distance = Distance.FromMeters(1000);
        
        // Act
        var result = costCalculator.Calculate(distance);

        // Assert
        Assert.Equal(1.8m, result);  
    }
    
    [Fact]
    public async Task Calculate_WithDefaultCoef_CalculateDistanceCostPerKm()
    {
        // Arrange
        var settings = new OrderSettings
        {
            CostPerKm = null
        };
        var costCalculator = new CostCalculator(settings); 
        var distance = Distance.FromMeters(1000);
        
        // Act
        var result = costCalculator.Calculate(distance);

        // Assert
        Assert.Equal(1m, result);  
    }
}
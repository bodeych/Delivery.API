using Delivery.API.Application.Settings;
using Delivery.API.Domain.ValueObjects;

namespace Delivery.API.Application.Services;

public class CostCalculator
{
    private const float DefaultCoefficient = 1f;

    private readonly OrderSettings _orderSettings;

    public CostCalculator(OrderSettings orderSettings)
    {
        _orderSettings = orderSettings;
    }

    public decimal Calculate(Distance distance)
    {
        var coefficient = _orderSettings.CostPerKm ?? DefaultCoefficient;

        var result = distance.Kilometers * coefficient;

        var cost = Convert.ToDecimal(result);

        return cost;
    }
}
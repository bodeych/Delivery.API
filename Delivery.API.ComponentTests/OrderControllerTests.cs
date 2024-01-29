using System.Text;
using Delivery.API.Controllers.Contracts.Requests;
using Delivery.API.Controllers.Contracts.Shared;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;

namespace Delivery.API.ComponentTests;

public class OrderControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrderControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    //
    // [Fact]
    // public async Task CreateOrder_ValidData_ReturnsOkResult()
    // {
    //     // Arrange
    //     var createOrderRequest = new CreateOrderRequest
    //     {
    //         PickUp = new Coordinate
    //         {
    //             Latitude = 10.0,
    //             Longitude = 11.0
    //         },
    //         DropOff = new Coordinate
    //         {
    //             Latitude = 12.0,
    //             Longitude = 12.0
    //         }
    //     };
    //     _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessTokenManager.AccessToken}");
    //
    //     var jsonContent = new StringContent(JsonConvert.SerializeObject(createOrderRequest), Encoding.UTF8, "application/json");
    //
    //     // Act
    //     var response = await _client.PostAsync("/api/v1/orders", jsonContent);
    //
    //     // Assert
    //     response.EnsureSuccessStatusCode();
    // }
}
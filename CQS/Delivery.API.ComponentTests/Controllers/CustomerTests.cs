using FluentAssertions;

namespace Delivery.API.ComponentTests.Controllers;

public class CustomerTests
{
    private readonly IServiceProvider _serviceProvider;

    public CustomerTests()
    {
        var sc = new ServiceCollection();

        sc.AddHttpClient();

        _serviceProvider = sc.BuildServiceProvider();
    }
    
    [Fact]
    public async Task GetAll_NotAuthorization_ReturnUnauthorized401()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();
        
        //Act
        var response = await client.GetAsync($"http://localhost:5248/api/v1/customer/orders");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetAll_OrdersIsNull_ReturnNotFound404()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var token = await new UserBuilder().WithLogin("1111").WithPassword("1111").Build();
        
        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {token.AccessToken}");
        
        //Act
        var response = await client.GetAsync($"http://localhost:5248/api/v1/customer/orders");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetAll_OrdersExist_ReturnListOrders()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var token = await new UserBuilder().Build();
        
        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {token.AccessToken}");
        
        var createOrderRequest = new CreateOrderRequest
        {
            Pickup = new Coordinate
            {
                Latitude = 10.0,
                Longitude = 11.0
            },
            Dropoff = new Coordinate
            {
                Latitude = 12.0,
                Longitude = 12.0
            }
        };
    
        var firstOrder = new StringContent(JsonConvert.SerializeObject(createOrderRequest), Encoding.UTF8, "application/json");
        await client.PostAsync($"http://localhost:5248/api/v1/order", firstOrder);
        var secondOrder = new StringContent(JsonConvert.SerializeObject(createOrderRequest), Encoding.UTF8, "application/json");
        await client.PostAsync($"http://localhost:5248/api/v1/order", secondOrder);
        
        //Act
        var response = await client.GetAsync($"http://localhost:5248/api/v1/customer/orders");
        
        //Assert
        response.Should().NotBeNull();
    }
}
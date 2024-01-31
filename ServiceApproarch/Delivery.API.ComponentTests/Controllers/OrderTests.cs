namespace Delivery.API.ComponentTests.Controllers;

public class OrderTests
{
    private readonly IServiceProvider _serviceProvider;

    public OrderTests()
    {
        var sc = new ServiceCollection();

        sc.AddHttpClient();

        _serviceProvider = sc.BuildServiceProvider();
    }
    [Fact]
    public async Task Post_NotAuthorization_ReturnUnauthorized401()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        
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
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createOrderRequest), Encoding.UTF8, "application/json");
        
        //Act
        var response = await client.PostAsync($"http://localhost:5139/api/v1/order", jsonContent);
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_CreateOrder_ReturnOrderId()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
    
        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();
    
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
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createOrderRequest), Encoding.UTF8, "application/json");
        
        //Act
        var response = await client.PostAsync($"http://localhost:5139/api/v1/order", jsonContent);
        var str = await response.Content.ReadAsStringAsync();
        var orderIdResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(str);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(orderIdResponse);
        });
    }
    
    [Fact]
    public async Task Post_RequestIsNull_ReturnBadRequest400()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
    
        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();
    
        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {token.AccessToken}");

        var createOrderRequest = new CreateOrderRequest();
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createOrderRequest), Encoding.UTF8, "application/json");
        
        //Act
        var response = await client.PostAsync($"http://localhost:5139/api/v1/order", jsonContent);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_PickupIsNull_ReturnBadRequest400()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
    
        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();
    
        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {token.AccessToken}");

        var createOrderRequest = new CreateOrderRequest
        {
            Pickup = null,
            Dropoff = new Coordinate
            {
                Latitude = 12.0,
                Longitude = 12.0
            }
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createOrderRequest), Encoding.UTF8, "application/json");
        
        //Act
        var response = await client.PostAsync($"http://localhost:5139/api/v1/order", jsonContent);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_DropoffIsNull_ReturnBadRequest400()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
    
        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();
    
        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {token.AccessToken}");

        var createOrderRequest = new CreateOrderRequest
        {
            Pickup = new Coordinate
            {
                Latitude = 12.0,
                Longitude = 12.0
            },
            Dropoff = null
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createOrderRequest), Encoding.UTF8, "application/json");
        
        //Act
        var response = await client.PostAsync($"http://localhost:5139/api/v1/order", jsonContent);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task GetById_OrderDoesExist_ReturnOrderDetails() 
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();
        
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
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createOrderRequest), Encoding.UTF8, "application/json");
        var orderResponse = await client.PostAsync($"http://localhost:5139/api/v1/order", jsonContent);
        var str = await orderResponse.Content.ReadAsStringAsync();
        var orderIdResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(str);
        
        //Act
        var response = await client.GetAsync($"http://localhost:5139/api/v1/order/{orderIdResponse.Id}");
        var newStr = await response.Content.ReadAsStringAsync();
        var order = JsonConvert.DeserializeObject<OrderDetailsResponse>(newStr);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(orderIdResponse.Id, order.Id);
            Assert.Equal(createOrderRequest.Pickup.Latitude, order.Pickup.Latitude);
            Assert.Equal(createOrderRequest.Pickup.Longitude, order.Pickup.Longitude);
            Assert.Equal(createOrderRequest.Dropoff.Latitude, order.Dropoff.Latitude);
            Assert.Equal(createOrderRequest.Dropoff.Longitude, order.Dropoff.Longitude);
        });
    }
    
    [Fact]
    public async Task GetById_NotAuthorization_ReturnUnauthorized401()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();

        var orderId = Guid.NewGuid();
        
        //Act
        var response = await client.GetAsync($"http://localhost:5139/api/v1/order/{orderId}");
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetById_OrderDoesNotExist_ReturnNotFound404()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        var orderId = Guid.NewGuid();

        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();

        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {token.AccessToken}");
        
        //Act
        var response = await client.GetAsync($"http://localhost:5139/api/v1/order/{orderId}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetById_OrderIdEmpty_BadRequest400()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        var orderId = Guid.Empty;

        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();

        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {token.AccessToken}");
        
        //Act
        var response = await client.GetAsync($"http://localhost:5139/api/v1/order/{orderId}");
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteById_OrderDoesExist_ReturnOk200()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();

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
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createOrderRequest), Encoding.UTF8, "application/json");
        var orderResponse = await client.PostAsync($"http://localhost:5139/api/v1/order", jsonContent);
        var str = await orderResponse.Content.ReadAsStringAsync();
        var orderIdResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(str);
        
        //Act
        var deleteResponse = await client.DeleteAsync($"http://localhost:5139/api/v1/order/{orderIdResponse.Id}");
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteById_NotAuthorization_ReturnUnauthorized401()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();

        var orderId = Guid.NewGuid();
        
        //Act
        var deleteResponse = await client.DeleteAsync($"http://localhost:5139/api/v1/order/{orderId}");
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, deleteResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteById_OrderIdEmpty_ReturnBadRequest400()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();
        
        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {token.AccessToken}");

        var orderId = Guid.Empty;
        
        //Act
        var deleteResponse = await client.DeleteAsync($"http://localhost:5139/api/v1/order/{orderId}");
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, deleteResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteById_OrderDoesNotExist_ReturnNotFound404()
    {
        //Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();
        
        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {token.AccessToken}");

        var orderId = Guid.NewGuid();
        
        //Act
        var deleteResponse = await client.DeleteAsync($"http://localhost:5139/api/v1/order/{orderId}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
    }
}
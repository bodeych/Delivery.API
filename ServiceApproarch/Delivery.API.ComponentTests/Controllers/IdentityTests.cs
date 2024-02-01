namespace Delivery.API.ComponentTests.Controllers;

public class IdentityTests
{
    private readonly IServiceProvider _serviceProvider;

    public IdentityTests()
    {
        var sc = new ServiceCollection();

        sc.AddHttpClient();

        _serviceProvider = sc.BuildServiceProvider();
    }
    
    

    [Fact]
    public async Task Registration_ValidData_ReturnOk()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        var registrationModel = new UserRegistrationRequest
        {
            Username = Guid.NewGuid().ToString(),
            Password = "validPassword"
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(registrationModel), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/registration", jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Registration_UserDoesExist_ReturnBadRequest()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();
        
        var createRegistrationRequest = new UserRegistrationRequest
        {
            Username = "1234",
            Password = "1234"
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createRegistrationRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/registration", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Registration_UsernameIsNull_ReturnBadRequest()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        
        var createRegistrationRequest = new UserRegistrationRequest
        {
            Username = null,
            Password = "valid"
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createRegistrationRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/registration", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Registration_PasswordIsNull_ReturnBadRequest()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        
        var createRegistrationRequest = new UserRegistrationRequest
        {
            Username = "valid",
            Password = null
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createRegistrationRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/registration", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Registration_RequestIsNull_ReturnBadRequest()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var createRegistrationRequest = new UserRegistrationRequest();
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(createRegistrationRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/registration", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Login_UserDoesExist_ReturnsOKAndTokens()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();
        
        var userLoginRequest = new UserLoginRequest
        {
            Username = "1234",
            Password = "1234"
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(userLoginRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/login", jsonContent);
        var str = await response.Content.ReadAsStringAsync();
        var loginResponseObj = JsonConvert.DeserializeObject<UserLoginResponse>(str);
    
        // Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(loginResponseObj.AccessToken);
            Assert.NotNull(loginResponseObj.RefreshToken);
            Assert.Equal(token.AccessToken, loginResponseObj.AccessToken);
            Assert.Equal(token.RefreshToken, loginResponseObj.RefreshToken);
        });
    }
    
    [Fact]
    public async Task Login_UserDoesNotExist_ReturnBadRequest400()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        
        var userLoginRequest = new UserLoginRequest
        {
            Username = "invalid",
            Password = "invalid"
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(userLoginRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/login", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Login_UsernameIsNull_ReturnBadRequest400()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        
        var userLoginRequest = new UserLoginRequest
        {
            Username =  null,
            Password = "valid"
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(userLoginRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/login", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Login_PasswordIsNull_ReturnBadRequest400()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        
        var userLoginRequest = new UserLoginRequest
        {
            Username =  "valid",
            Password = null
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(userLoginRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/login", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Login_RequestIsNull_ReturnBadRequest400()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var userLoginRequest = new UserLoginRequest();
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(userLoginRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/login", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Refresh_ValidData_ReturnOK200AndTokens()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        var token = await new UserBuilder().WithLogin("1234").WithPassword("1234").Build();

        var tokenRequest = new TokenRefreshRequest
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/refresh", jsonContent);
        var str = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<TokenRefreshResponse>(str);
    
        // Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(tokenResponse.AccessToken);
            Assert.NotNull(tokenResponse.RefreshToken);
            Assert.NotEqual(token.AccessToken, tokenResponse.AccessToken);
        });
    }
    
    [Fact]
    public async Task Refresh_InvalidData_ReturnBadRequest400()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var tokenRequest = new TokenRefreshRequest
        {
            AccessToken = "invalid",
            RefreshToken = "invalid"
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/refresh", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Refresh_AccessTokenIsNull_ReturnBadRequest400()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var tokenRequest = new TokenRefreshRequest
        {
            AccessToken = null,
            RefreshToken = "valid"
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/refresh", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Refresh_RefreshTokensNull_ReturnBadRequest400()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var tokenRequest = new TokenRefreshRequest
        {
            AccessToken = "valid",
            RefreshToken = null
        };
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/refresh", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Refresh_RequestIsNull_ReturnBadRequest400()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();

        var tokenRequest = new TokenRefreshRequest();
    
        var jsonContent = new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync("http://localhost:5139/api/v1/identity/refresh", jsonContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
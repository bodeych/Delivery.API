using System.Net;
using System.Text;
using Delivery.API.Controllers.Contracts.Requests;
using Delivery.API.Controllers.Contracts.Responses;
using Newtonsoft.Json;

namespace Delivery.API.ComponentTests;

public class IdentityControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IdentityControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    

    [Fact]
    public async Task Registation_ValidData_ReturnsOkResult()
    {
        // Arrange
        var createRegistrationRequest = new UserRegistrationRequest
        {
            Username = "12345",
            Password = "12345"
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(createRegistrationRequest), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("api/v1/registration", jsonContent);

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task Registation_DuplicateUsername_ReturnsBadRequest()
    {
        // Arrange
        var existingUsername = "existingUser";
        
        var createRegistrationRequest = new UserRegistrationRequest
        {
            Username = existingUsername,
            Password = "12345"
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(createRegistrationRequest), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("api/v1/registration", jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Login_ExistingUser_ReturnsOKAndTokens()
    {
        // Arrange
        var existingUsername = "existingUser";
        
        var userLoginRequest = new UserLoginRequest
        {
            Username = existingUsername,
            Password = "12345"
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(userLoginRequest), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("api/v1/login", jsonContent);

        // Assert\
       // response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonConvert.DeserializeObject<UserLoginResponse>(responseContent);

        Assert.NotNull(loginResponse.AccessToken);
        Assert.NotNull(loginResponse.RefreshToken);
    }
    
    // [Fact]
    // public async Task Login_ValidData_ReturnsOkResultAndTokens()
    // {
    //     // Arrange
    //     var userBuilder = new UserBuilder(_client)
    //         .WithUsername("newUser")
    //         .WithPassword("newPassword");
    //
    //     // Act
    //     var user = userBuilder.Build();
    //     // Assert
    //
    //     Assert.NotNull(user.AccessToken);
    //     Assert.NotNull(user.RefreshToken);
    // }
}
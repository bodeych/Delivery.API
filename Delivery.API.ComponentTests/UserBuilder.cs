using System.Net.Http.Json;
using System.Text;
using Delivery.API.Controllers.Contracts.Requests;
using Delivery.API.Controllers.Contracts.Responses;
using Delivery.API.Domain.Entities;
using Newtonsoft.Json;

namespace Delivery.API.ComponentTests;

public class UserBuilder
{
    private readonly HttpClient _client;
    private string _username;
    private string _password;
    private string _accessToken;
    private string _refreshToken;

    public UserBuilder(HttpClient client)
    {
        _client = client;
    }

    public UserBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public User Build()
    {
        var registrationRequest = new UserRegistrationRequest
        {
            Username = _username,
            Password = _password
        };

        // Register user
        var registrationResponse = _client.PostAsJsonAsync("/api/v1/registration", registrationRequest).Result;
        registrationResponse.EnsureSuccessStatusCode();

        var loginRequest = new UserLoginRequest
        {
            Username = _username,
            Password = _password
        };

        // Login user
        var loginResponse = _client.PostAsJsonAsync("/api/v1/login", loginRequest).Result;
        loginResponse.EnsureSuccessStatusCode();

        var loginResponseContent = loginResponse.Content.ReadAsStringAsync().Result;
        var loginResponseObj = JsonConvert.DeserializeObject<UserLoginResponse>(loginResponseContent);

        _accessToken = loginResponseObj.AccessToken;
        _refreshToken = loginResponseObj.RefreshToken;

        //return User.Build(_username, _password, _accessToken, _refreshToken);
    }
}
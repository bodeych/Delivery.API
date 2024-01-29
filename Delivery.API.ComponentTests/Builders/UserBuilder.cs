namespace Delivery.API.ComponentTests.Builders;

public class UserBuilder
{
    private readonly IServiceProvider _serviceProvider;
    private string Login { get; set; } = Guid.NewGuid().ToString();
    private string Password { get; set; } = Guid.NewGuid().ToString();

    public UserBuilder()
    {
        var sc = new ServiceCollection();

        sc.AddHttpClient();

        _serviceProvider = sc.BuildServiceProvider();
    }

    public UserBuilder WithLogin(string login)
    {
        Login = login;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        Password = password;
        return this;
    }

    public async Task<TokenResponse> Build()
    {
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient();
        var registrationModel = new UserRegistrationRequest
        {
            Username = Login,
            Password = Password
        };

        var jsonContentRegister = new StringContent(JsonConvert.SerializeObject(registrationModel), Encoding.UTF8,
            "application/json");

        var responseRegister = await client.PostAsync("http://localhost:5139/api/v1/identity/registration", jsonContentRegister);

        var loginRequest = new UserLoginRequest
        {
            Username = Login,
            Password = Password
        };

        var jsonContentLogin = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8,
            "application/json");

        var responseLogin = await client.PostAsync("http://localhost:5139/api/v1/identity/login", jsonContentLogin);

        var str = await responseLogin.Content.ReadAsStringAsync();
        var loginResponseObj = JsonConvert.DeserializeObject<UserLoginResponse>(str);

        return new TokenResponse
        {
            AccessToken = loginResponseObj.AccessToken,
            RefreshToken = loginResponseObj.RefreshToken
        };

    }
}

[DataContract]
public class TokenResponse
{
    [DataMember(Name = "access_token")]
    public string AccessToken { get; init; }
    [DataMember(Name = "refresh_token")]
    public string RefreshToken { get; init; }
}

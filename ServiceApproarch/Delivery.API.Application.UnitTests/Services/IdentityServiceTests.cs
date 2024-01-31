namespace Delivery.API.Application.UnitTests.Services;

using BCrypt.Net;

public class IdentityServiceTests
{
    [Fact]
    public async Task Login_ValidUser_ReturnTokens()
    {
        // Arrange
        var fakeIdentityRepository = new FakeIdentityRepository();
        var jwtSettings = new JwtSettings();
        var generateToken = new GenerateToken(jwtSettings);
        var identityService = new IdentityService(fakeIdentityRepository, generateToken);
         
        var loginDto = new UserLoginDto
        {
            Username = "test1",
            Password = "test1"
        };

        var hashedPassword = BCrypt.HashPassword(loginDto.Password);
        var existingUser = User.Create(Guid.NewGuid(), loginDto.Username, hashedPassword, "AccessToken", "RefreshToken");
         
        fakeIdentityRepository.Users.Add(existingUser);
        
        // Act
        var tokens = await identityService.Login(loginDto, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(tokens);
            Assert.Equal(tokens.AccessToken, existingUser.AccessToken);
            Assert.Equal(tokens.RefreshToken, existingUser.RefreshToken);
        });
    }
    
    [Fact]
    public async Task Login_InvalidUser_ReturnNull()
    {
        // Arrange
        var fakeIdentityRepository = new FakeIdentityRepository();
        var jwtSettings = new JwtSettings();
        var generateToken = new GenerateToken(jwtSettings);
        var identityService = new IdentityService(fakeIdentityRepository, generateToken);
         
        var loginDto = new UserLoginDto
        {
            Username = "test2",
            Password = "test2"
        };
        
        // Act
        var tokens = await identityService.Login(loginDto, CancellationToken.None);

        // Assert
        Assert.Null(tokens);
    }
    
    [Fact]
    public async Task Register_NewUser_SuccessfulRegistrationReturnTrue()
    {
        // Arrange
        var fakeIdentityRepository = new FakeIdentityRepository();
        var jwtSettings = new JwtSettings
        {
            SecretKey = "your-secret-key-1-2-3-4-5-6-7-8-9-0"
        };
        var generateToken = new GenerateToken(jwtSettings);
        var identityService = new IdentityService(fakeIdentityRepository, generateToken);
        
        var registrationDto = new UserRegistrationDto()
        {
            Username = "test3",
            Password = "test3"
        };
        
        // Act
        var result = await identityService.Register(registrationDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task Register_NewUser_UnsuccessfulRegistrationReturnFalse()
    {
        // Arrange
        var fakeIdentityRepository = new FakeIdentityRepository();
        var jwtSettings = new JwtSettings
        {
            SecretKey = "your-secret-key-1-2-3-4-5-6-7-8-9-0"
        };
        var generateToken = new GenerateToken(jwtSettings);
        var identityService = new IdentityService(fakeIdentityRepository, generateToken);
        
        var hashedPassword = BCrypt.HashPassword("test");
        var existingUser = User.Create(Guid.NewGuid(), "test", hashedPassword, "AccessToken", "RefreshToken");
         
        fakeIdentityRepository.Users.Add(existingUser);
        
        var registrationDto = new UserRegistrationDto()
        {
            Username = "test",
            Password = "test"
        };
        
        // Act
        var result = await identityService.Register(registrationDto, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task Refresh_ValidToken_ReturnNewAccessToken()
    {
        // Arrange
        var fakeIdentityRepository = new FakeIdentityRepository();
        var jwtSettings = new JwtSettings
        {
            SecretKey = "your-secret-key-1-2-3-4-5-6-7-8-9-0"
        };
        var generateToken = new GenerateToken(jwtSettings);
        var identityService = new IdentityService(fakeIdentityRepository, generateToken);
        
        var hashedPassword = BCrypt.HashPassword("test5");
        var tokensDto = new TokensDto()
        {
            AccessToken = "AccessToken",
            RefreshToken = "RefreshToken"
        };
        var existingUser = User.Create(Guid.NewGuid(), "test5", hashedPassword, tokensDto.AccessToken, tokensDto.RefreshToken);
         
        fakeIdentityRepository.Users.Add(existingUser);
        
        // Act
        var newTokens = await identityService.Refresh(tokensDto, CancellationToken.None);

        // Assert
        Assert.NotEqual(tokensDto.AccessToken, newTokens.AccessToken);
    }
    
    [Fact]
    public async Task Refresh_invalidToken_ReturnNull()
    {
        // Arrange
        var fakeIdentityRepository = new FakeIdentityRepository();
        var jwtSettings = new JwtSettings
        {
            SecretKey = "your-secret-key-1-2-3-4-5-6-7-8-9-0"
        };
        var generateToken = new GenerateToken(jwtSettings);
        var identityService = new IdentityService(fakeIdentityRepository, generateToken);
        
        var tokensDto = new TokensDto()
        {
            AccessToken = "AccessToken",
            RefreshToken = "RefreshToken"
        };
        
        // Act
        var newTokens = await identityService.Refresh(tokensDto, CancellationToken.None);

        // Assert
        Assert.Null(newTokens);
    }
}
namespace Delivery.API.Application.Dto;

public sealed class TokensDto
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}
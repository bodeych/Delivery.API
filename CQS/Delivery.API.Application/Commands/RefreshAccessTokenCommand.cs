using Delivery.API.Application.Dto;
using Delivery.API.Application.Interfaces;
using Delivery.API.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Application.Commands;

public class RefreshAccessTokenCommand : ICommand<TokensDto?>
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}

internal sealed class RefreshAccessTokenCommandHandler : ICommandHandler<RefreshAccessTokenCommand, TokensDto?>
{
    private readonly IDataContext _dataContext;
    private readonly GenerateToken _generateToken;
    
    public RefreshAccessTokenCommandHandler(IDataContext dataContext, GenerateToken generateToken)
    {
        _dataContext = dataContext;
        _generateToken = generateToken;
    }

    public async Task<TokensDto?> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccessToken == request.AccessToken, cancellationToken: cancellationToken);
        if (user is null || user.RefreshToken != request.RefreshToken)
        {
            return null;
        }
         
        var id = user.UserId;
         
        var newAccessToken = _generateToken.AccessToken(id);
        user.UpdateAccessToken(newAccessToken);
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync(cancellationToken);
         
        var tokensDto = new TokensDto
        {
            AccessToken = user.AccessToken,
            RefreshToken = user.RefreshToken
        };

        return tokensDto;
    }
}
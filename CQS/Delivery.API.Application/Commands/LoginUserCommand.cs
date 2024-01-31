using Delivery.API.Application.Dto;
using Delivery.API.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Application.Commands;

using BCrypt.Net;
public class LoginUserCommand : ICommand<TokensDto?>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, TokensDto?>
{
    private readonly IDataContext _dataContext;
    
    public LoginUserCommandHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<TokensDto?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken: cancellationToken);

        if (user == null || BCrypt.Verify(request.Password, user.Password) == false)
        {
            return null;
        }

        var tokensDto = new TokensDto
        {
            AccessToken = user.AccessToken,
            RefreshToken = user.RefreshToken
        };

        return tokensDto;
    }
}
using Delivery.API.Application.Interfaces;
using Delivery.API.Application.Service;
using Delivery.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Application.Commands;

using BCrypt.Net;
public class RegistrationUserCommand : ICommand<bool>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

internal sealed class RegistrationUserCommandHandler : ICommandHandler<RegistrationUserCommand, bool>
{
    private readonly IDataContext _dataContext;
    private readonly GenerateToken _generateToken;
    
    public RegistrationUserCommandHandler(IDataContext dataContext, GenerateToken generateToken)
    {
        _dataContext = dataContext;
        _generateToken = generateToken;
    }

    public async Task<bool> Handle(RegistrationUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken: cancellationToken);
        if (existingUser != null)
        {
            return false;
        }

        var id = Guid.NewGuid();
        var user = request.Username;
        var password = BCrypt.HashPassword(request.Password);
        var accessToken = _generateToken.AccessToken(id);
        var refreshToken = _generateToken.RefreshToken();
        var createdUser = User.Create(id, user, password, accessToken, refreshToken);

        _dataContext.Users.Add(createdUser);
        await _dataContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
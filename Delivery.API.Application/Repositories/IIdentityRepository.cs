using Delivery.API.Domain.Entities;

namespace Delivery.API.Application.Repositories;

public interface IIdentityRepository
{
    Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> FindByTokenAsync(string accessToken, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
}
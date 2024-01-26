using Delivery.API.Application.Repositories;
using Delivery.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Infrastructure.Repositories;

public class IdentityRepository : IIdentityRepository
{
    private readonly DataContext _dataContext;

    public IdentityRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken: cancellationToken);
    }
    
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> FindByTokenAsync(string accessToken, CancellationToken cancellationToken)
    {
        return await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccessToken == accessToken, cancellationToken: cancellationToken);
    }
    
    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}
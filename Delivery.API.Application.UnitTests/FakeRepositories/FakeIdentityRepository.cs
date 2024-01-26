namespace Delivery.API.Application.UnitTests.FakeRepositories;

public class FakeIdentityRepository : IIdentityRepository
{
    public List<User> Users { get; } = new List<User>();

    public Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var user = Users.Find(x => x.Username == username);
        return Task.FromResult(user);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        Users.Add(user);
        return Task.CompletedTask;
    }

    public Task<User?> FindByTokenAsync(string accessToken, CancellationToken cancellationToken)
    {
        var user = Users.Find(x => x.AccessToken == accessToken);

        return Task.FromResult<User?>(user);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var existingUser = Users.Find(x => x.UserId == user.UserId);
        if(existingUser != null)
        {
            existingUser.UpdateAccessToken(user.AccessToken);
        }
        return Task.CompletedTask;
    }
}
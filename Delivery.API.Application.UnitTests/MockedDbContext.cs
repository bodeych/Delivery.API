using System.Linq.Expressions;
using Delivery.API.Application.Interfaces;
using Delivery.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Delivery.API.Application.UnitTests;

public class MockedDbContext : IDataContext
{
    public DbSet<Order> Orders => new MyDbSet<Order>();
    
    public DbSet<User> Users => new MyDbSet<User>();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

internal sealed class MyDbSet<T> : DbSet<T>
    where T : class
{
    private readonly List<T> _data = new List<T>();
    public override IEntityType EntityType { get; }

    public override EntityEntry<T> Add(T entity)
    {
         _data.Add(entity);
         return 
    }
    

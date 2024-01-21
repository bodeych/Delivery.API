using Delivery.API.Domain;
using Delivery.API.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Application.Interfaces;

public interface IDataContext
{
    DbSet<Order> Orders { get; }
    
    public DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
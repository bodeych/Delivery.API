using Delivery.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Application.Interfaces;

public interface IDataContext
{
    DbSet<Order> Orders { get; }

    DbSet<User> Users { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
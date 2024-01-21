using Delivery.API.Domain;
using Delivery.API.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Application.Interfaces;

public interface IDataContext
{
    DbSet<Order> Orders { get; }
    
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
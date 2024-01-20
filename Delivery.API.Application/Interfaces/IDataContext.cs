using Delivery.API.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Application.Interfaces;

public interface IDataContext
{
    DbSet<Order> Orders { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
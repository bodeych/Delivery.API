using Delivery.API.Application.Repositories;
using Delivery.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DataContext _dataContext;

    public OrderRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        _dataContext.Orders.Add(order);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Order?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dataContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public async Task RemoveAsync(Order order, CancellationToken cancellationToken)
    {
        _dataContext.Orders.Remove(order);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}
using Delivery.API.Domain.Entities;

namespace Delivery.API.Application.Repositories;

public interface IOrderRepository 
{
        Task AddAsync(Order order, CancellationToken cancellationToken);
        Task<Order?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
        Task RemoveAsync(Order order, CancellationToken cancellationToken);
}
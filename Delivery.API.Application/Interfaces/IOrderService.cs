using Delivery.API.Application.Dto;

namespace Delivery.API.Application.Interfaces;

public interface IOrderService
{
    Task<Guid> Create(CreateOrderDto dto, CancellationToken cancellationToken);
    Task<OrderDetailsDto?> FindById(Guid id, CancellationToken cancellationToken);
    Task<bool> DeleteById(Guid id, Guid userId, CancellationToken cancellationToken);
}
using Delivery.API.Application.Dto;

namespace Delivery.API.Application.Interfaces;

public interface ICustomerService
{
    Task<List<OrderDetailsDto>> FindById(Guid id, CancellationToken cancellationToken);
}
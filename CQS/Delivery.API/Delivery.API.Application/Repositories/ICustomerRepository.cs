using Delivery.API.Domain.Entities;

namespace Delivery.API.Application.Repositories;

public interface ICustomerRepository
{
    Task<List<Order>> FindByIdToListAsync(Guid id);
}
using Delivery.API.Application.Repositories;
using Delivery.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly DataContext _dataContext;

    public CustomerRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Order>> FindByIdToListAsync(Guid id)
    {
        return await _dataContext.Orders
            .AsNoTracking()
            .Where(i => i.UserId == id).ToListAsync();
    }
}
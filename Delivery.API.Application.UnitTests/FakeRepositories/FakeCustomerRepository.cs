namespace Delivery.API.Application.UnitTests.FakeRepositories;

public class FakeCustomerRepository : ICustomerRepository
{
    public List<Order> Orders { get; } = new List<Order>();
    public Task<List<Order>> FindByIdToListAsync(Guid id)
    {
        var list = Orders.Where(i => i.UserId == id).ToList();
        return Task.FromResult(list);
    }
}
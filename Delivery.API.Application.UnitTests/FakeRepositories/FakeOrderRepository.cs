namespace Delivery.API.Application.UnitTests.FakeRepositories;

public class FakeOrderRepository : IOrderRepository
{
        public List<Order> Orders { get; } = new List<Order>();
         
        public Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            Orders.Add(order);
            return Task.CompletedTask;
        }
     
        public Task<Order?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var order = Orders.Find(x => x.Id == id);
            return Task.FromResult(order);
        }
     
        public Task RemoveAsync(Order order, CancellationToken cancellationToken)
        {
            Orders.Remove(order);
            return Task.CompletedTask;
        }
}
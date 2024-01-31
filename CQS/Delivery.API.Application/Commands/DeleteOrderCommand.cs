using Delivery.API.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Application.Commands;

public sealed class DeleteOrderCommand : ICommand<bool>
{
    public required Guid Id { get; init; }
    public Guid UserId { get; init; }
}

internal sealed class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand, bool>
{
    private readonly IDataContext _dataContext;
    
    public DeleteOrderCommandHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _dataContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (order is null || order.UserId != request.UserId)
        {
            return false;
        }

        _dataContext.Orders.Remove(order);
        await _dataContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
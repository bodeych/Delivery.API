using Delivery.API.Controllers.Contracts.Requests;
using FluentValidation;

namespace Delivery.API.Controllers.Validators;

public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x).NotNull();
        RuleFor(x => x.Pickup).NotNull();
        RuleFor(x => x.Dropoff).NotNull();
    }
}
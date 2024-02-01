using FluentValidation;

namespace Delivery.API.Controllers.Validators;

public class GuidRequestValidator: AbstractValidator<Guid>
{
    public GuidRequestValidator()
    {
        RuleFor(x => x).NotEmpty();
    }
}
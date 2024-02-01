using Delivery.API.Controllers.Contracts.Requests;
using FluentValidation;

namespace Delivery.API.Controllers.Validators;

public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationRequestValidator()
    {
        RuleFor(x => x).NotNull();
        RuleFor(x => x.Username).NotNull();
        RuleFor(x => x.Password).NotNull();
    }
}
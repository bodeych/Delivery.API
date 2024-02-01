using Delivery.API.Controllers.Contracts.Requests;
using FluentValidation;

namespace Delivery.API.Controllers.Validators;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x).NotNull();
        RuleFor(x => x.Username).NotNull();
        RuleFor(x => x.Password).NotNull();
    }
}
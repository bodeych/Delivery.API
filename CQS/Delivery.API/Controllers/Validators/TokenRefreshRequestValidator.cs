using Delivery.API.Controllers.Contracts.Requests;
using FluentValidation;

namespace Delivery.API.Controllers.Validators;

public class TokenRefreshRequestValidator : AbstractValidator<TokenRefreshRequest>
{
    public TokenRefreshRequestValidator()
    {
        RuleFor(x => x).NotNull();
        RuleFor(x => x.AccessToken).NotNull();
        RuleFor(x => x.RefreshToken).NotNull();
    }
}
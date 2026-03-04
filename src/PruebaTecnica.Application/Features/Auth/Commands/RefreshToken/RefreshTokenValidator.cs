using FluentValidation;

namespace PruebaTecnica.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.Request.AccessToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("AccessToken is required.");

        RuleFor(x => x.Request.RefreshToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("RefreshToken is required.");
    }
}

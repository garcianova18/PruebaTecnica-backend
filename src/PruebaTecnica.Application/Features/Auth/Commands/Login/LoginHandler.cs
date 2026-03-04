using FluentValidation;
using MediatR;
using PruebaTecnica.Application.Common.Exceptions;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Application.Contracts.Services;
using PruebaTecnica.Application.DTOs.Auth;
using ValidationException = PruebaTecnica.Application.Common.Exceptions.ValidationException;


namespace PruebaTecnica.Application.Features.Auth.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IPasswordHasherService passwordHasher;
    private readonly IJwtService tokenService;
    private readonly IValidator<LoginRequest> validator;

    public LoginHandler(IUnitOfWork unitOfWork, 
        IPasswordHasherService passwordHasher,
        IJwtService tokenService,
        IValidator<LoginRequest> validator)
    {
        this.unitOfWork = unitOfWork;
        this.passwordHasher = passwordHasher;
        this.tokenService = tokenService;
        this.validator = validator;
    }
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

        var validationResult = await validator.ValidateAsync(request.LoginRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).First();
            throw new ValidationException(errors);
        }

        var user = await unitOfWork.Users.GetByUserNameWithRolesAsync(request.LoginRequest.UserName!, cancellationToken);

        if (user is null || !passwordHasher.VerifyPassword(request.LoginRequest.Password!,user.PasswordHash))
        {
           throw new UnauthorizedException("Usuario o contraseña incorrectos.");
        }

        var accessToken = tokenService.GenerateToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();

        return new AuthResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = 3600,
            Username = user.UserName,
            Email = user.Email,
            Role = user.Role.Name
        };
    }
}

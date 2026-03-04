using AutoMapper;
using FluentValidation;
using MediatR;
using PruebaTecnica.Application.Common.Exceptions;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Application.Contracts.Services;
using PruebaTecnica.Application.DTOs.Auth;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Features.Auth.Commands.Register;

public class RegisterHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasherService passwordHasher,
    IJwtService tokenService,
    IValidator<RegisterRequest> validator,
    IMapper mapper
) : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.RegisterRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).First();
            throw new BadRequestException(errors);
        }

        if (await unitOfWork.Users.UserNameExistsAsync(request.RegisterRequest.UserName!, cancellationToken))
        {
            throw new BadRequestException($"Username '{request.RegisterRequest.UserName}' ya esta registrado.");
        }

        if (await unitOfWork.Users.EmailExistsAsync(request.RegisterRequest.Email!, cancellationToken))
        {
            throw new BadRequestException($"Email '{request.RegisterRequest.Email}' ya esta registrado.");
        }
           
        var userEntity = mapper.Map<User>(request.RegisterRequest);
        userEntity.PasswordHash = passwordHasher.HashPassword(request.RegisterRequest.Password!);
        userEntity.RolId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var user = await unitOfWork.Users.AddAsync(userEntity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);


        var refreshToken = tokenService.GenerateRefreshToken();
        var accessToken = tokenService.GenerateToken(userEntity);

        return new AuthResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = 3600,
            Username = user.UserName,
            Email = user.Email,
        };
    }
}

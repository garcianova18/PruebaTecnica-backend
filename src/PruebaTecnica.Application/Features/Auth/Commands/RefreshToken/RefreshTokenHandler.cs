using MediatR;
using PruebaTecnica.Application.Common.Exceptions;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Application.Contracts.Services;
using PruebaTecnica.Application.DTOs.Auth;

namespace PruebaTecnica.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenHandler(
    IUnitOfWork unitOfWork,
    IJwtService tokenService
) : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Request;

        var principal = tokenService.GetPrincipalFromExpiredToken(dto.AccessToken!)
            ?? throw new UnauthorizedException("Invalid access token.");

        //var user = await unitOfWork.Users.GetByRefreshTokenAsync(dto.RefreshToken!, cancellationToken)
        //    ?? throw new UnauthorizedException("Invalid refresh token.");

        //if (user.RefreshTokenExpiry <= DateTime.UtcNow)
        //    throw new UnauthorizedException("Refresh token has expired.");

        var user = await unitOfWork.Users.GetWithRolesAsync(request.Request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with id '{request.Request.UserId}' was not found.");
        }

        var newAccessToken = tokenService.GenerateToken(user);
        var newRefreshToken = tokenService.GenerateRefreshToken();


        return new AuthResponse
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = 3600,
            Username = user.UserName,
            Email = user.Email,
            Role = user.Role.Name
        };
    }
}

using MediatR;
using PruebaTecnica.Application.DTOs.Auth;


namespace PruebaTecnica.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(RefreshTokenRequest Request) : IRequest<AuthResponse>;

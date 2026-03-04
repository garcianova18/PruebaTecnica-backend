using MediatR;
using PruebaTecnica.Application.DTOs.Auth;


namespace PruebaTecnica.Application.Features.Auth.Commands.Login;

public record LoginCommand(LoginRequest LoginRequest) : IRequest<AuthResponse>;

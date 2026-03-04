using MediatR;
using PruebaTecnica.Application.DTOs.Auth;


namespace PruebaTecnica.Application.Features.Auth.Commands.Register;

public record RegisterCommand(RegisterRequest RegisterRequest) : IRequest<AuthResponse>;

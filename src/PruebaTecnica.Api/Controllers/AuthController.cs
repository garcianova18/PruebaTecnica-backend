using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Common.Models;
using PruebaTecnica.Application.DTOs.Auth;
using PruebaTecnica.Application.Features.Auth.Commands.Login;
using PruebaTecnica.Application.Features.Auth.Commands.RefreshToken;
using PruebaTecnica.Application.Features.Auth.Commands.Register;



namespace PruebaTecnica.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRquest, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RegisterCommand(registerRquest), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, ApiResponse<AuthResponse>.Success(result, StatusCodes.Status201Created));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest , CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new LoginCommand(loginRequest), cancellationToken);
        return Ok(ApiResponse<AuthResponse>.Success(result));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest tokenRequest , CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RefreshTokenCommand(tokenRequest), cancellationToken);
        return Ok(ApiResponse<AuthResponse>.Success(result));
    }
}

using LibraryManagement.Application.Auth.Commands.Login;
using LibraryManagement.Application.Auth.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password), cancellationToken);
        return Ok(result);
        
    }
}
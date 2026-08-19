using Microsoft.AspNetCore.Mvc;
using Selecta.Api.Security;
using Selecta.Core.Dtos;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, IJwtTokenService jwtTokenService) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request, CancellationToken ct)
    {
        var user = await userService.ValidateCredentialsAsync(request.Login, request.Password, ct);
        if (user is null)
            return Unauthorized(new { message = "Login ou password inválidos." });

        var (token, expiresAt) = jwtTokenService.GenerateToken(user);
        return Ok(new LoginResponseDto(token, expiresAt, user));
    }
}

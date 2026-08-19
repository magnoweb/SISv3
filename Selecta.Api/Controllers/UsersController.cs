using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selecta.Core.Dtos;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Api.Controllers;

/// <summary>
/// Somente leitura — usado para popular seletores (ex.: "Manager" em Job
/// Openings). O CRUD de utilizadores (criação, alteração de perfil/senha)
/// fica fora do escopo por agora; a gestão de acesso já existe via login.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController(IUserService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));
}

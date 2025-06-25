using Microsoft.AspNetCore.Mvc;

namespace UsuariosApp.API.Controllers.V2;

[ApiController]
[Route("api/v2/[controller]")]
[ApiExplorerSettings(GroupName = "v2")]
public class UsuariosController : ControllerBase
{
    [HttpPost("autenticar")]
    public async Task<IActionResult> Autenticar()
    {
        return StatusCode(501, new { Mensagem = "Não implementado." });
    }
    [HttpPost("criar")]
    public async Task<IActionResult> Criar()
    {
        return StatusCode(501, new { Mensagem = "Não implementado." });
    }
}
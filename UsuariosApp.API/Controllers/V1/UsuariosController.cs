using Microsoft.AspNetCore.Mvc;
using UsuariosApp.API.Components;
using UsuariosApp.API.Entities;
using UsuariosApp.API.Repositories;

namespace UsuariosApp.API.Controllers.V1;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/[controller]")]
public class UsuariosController(UsuarioRepository usuarioRepository, JwtBearerComponent jwtBearerComponent, RabbitMQComponent rabbitMQComponent) : ControllerBase
{
    [HttpPost("autenticar")] //api/usuarios/autenticar
    public async Task<IActionResult> Autenticar([FromBody] AutenticarUsuarioRequest request)
    {
        try
        {
            var usuario = await usuarioRepository.Obter(request.Email, request.Senha);
            
            if (usuario == null)
                return Unauthorized(new { Message = "Usuário inválido. Acesso não autorizado!" });

            //sucesso
            return StatusCode(200, new
            {
                Mensagem = "Usuário autenticado com sucesso.",
                Usuario = new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                    Perfil = new
                    {
                        usuario.Perfil?.Id,
                        usuario.Perfil?.Nome
                    }
                },
                Token = jwtBearerComponent.CreateToken(usuario.Email, usuario.Perfil?.Nome),
                DataExpiracao = jwtBearerComponent.GetExpiration()
            });
        }
        catch (ApplicationException e)
        {
            return BadRequest(new { e.Message });
        }
    }

    [HttpPost("criar")] //api/usuarios/criar
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest request)
    {
        try
        {
            //criando usuário
            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = request.Senha
            };

            //gravando na base de dados
            var id = await usuarioRepository.Inserir(usuario);

            //criando evento
            var usuarioCriado = new UsuarioCriadoEvent(
                usuario.Nome, usuario.Email, DateTime.Now);

            //Envindo o evento para a fila RabbitMQ
            await rabbitMQComponent.Publish(usuarioCriado);

            return StatusCode(201, new { Message = "Usuário criado com sucesso.", 
            Usuario = new { 
                    Id = id,
                    usuario.Nome,
                    usuario.Email,
                }
            });
        }
        catch (ApplicationException e)
        {
            return BadRequest(new { e.Message });
        }
    }
}

#region  Record para definir os dados da requisição

public record AutenticarUsuarioRequest(
    string Email,
    string Senha
    );

public record CriarUsuarioRequest(
    string Nome,
    string Email,
    string Senha,
    string SenhaConfirmacao
);

#endregion

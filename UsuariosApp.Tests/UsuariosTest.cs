using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UsuariosApp.Tests;

public class UsuariosTest
{
    private readonly HttpClient _httpClient;
    private readonly Faker _faker;

    private const string _apiCriarUsuario = "/api/usuarios/criar";
    private const string _apiAutenticarUsuario = "/api/usuarios/autenticar";

    public UsuariosTest(HttpClient httpClient)
    {
        _httpClient = new WebApplicationFactory<Program>().CreateClient();

        _faker = new Faker("pt_BR");
    }

    [Fact(DisplayName = "Deve criar usuário com sucesso quando os dados forem válidos")]
    public void DeveCriarUsuario_QuandoDadosValidos()
    {
    }

    [Fact(DisplayName = "Deve retornar erro de requisição inválida quando email já existe")]
    public void DeveRetornarErro_QuandoEmailJaExiste()
    {
    }

    [Fact(DisplayName = "Deve autenticar usuário com sucesso quando as credenciais forem válidas")]
    public void DeveAutenticarUsuario_QuandoCredenciaisValidas()
    {
    }

    [Fact(DisplayName = "Deve retornar erro de acesso negado quando as credenciais forem inválidas")]
    public void DeveRetornarAcessoNegado_QuandoCredenciaisInvalidas()
    {
    }
}
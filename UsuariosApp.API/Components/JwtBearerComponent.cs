using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UsuariosApp.API.Components;

public class JwtBearerComponent(JwtSettings jwtSettings)
{
    /// <summary>
    /// Método para retornar a data e hora de expiração do token JWT.
    /// </summary>
    public DateTime GetExpiration()
        => DateTime.Now.AddMinutes(jwtSettings.Expiration);

    /// <summary>
    /// Método para retornar um token jwt
    /// </summary>
    public string CreateToken(string user, string role)
    {
        if (string.IsNullOrEmpty(jwtSettings.SecretKey))
            throw new Exception("Falha ao gerar o token.");

        // gerando a chave de assinatura criptografada para o token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // gerando os dados do usuário
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user),
            new Claim(ClaimTypes.Role, role),
        };

        //construindo o token
        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: GetExpiration(),
            signingCredentials: credentials);

        //retornando o token em formato string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

/// <summary>
/// Classe para capturar os parametros de configuração do JWT.
/// </summary>
public class JwtSettings
{
    public string? SecretKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int Expiration { get; set; }
}
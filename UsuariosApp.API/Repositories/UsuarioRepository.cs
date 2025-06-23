using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using UsuariosApp.API.Components;
using UsuariosApp.API.Entities;

namespace UsuariosApp.API.Repositories;

public class UsuarioRepository(string connectionString)
{
    public async Task<Usuario?> Obter(string email, string senha)
    {
        using var connection = new SqlConnection(connectionString);

        try
        {
            var query = @"SELECT u.ID, u.NOME, u.EMAIL, u.PERFILID,
                                p.ID, p.NOME
                          FROM USUARIO u
                          INNER JOIN PERFIL p
                          WHERE u.EMAIL = @Email AND u.Senha";

            var result = await connection.QueryAsync(query, (Usuario usuario, Perfil perfil) =>
            {
                usuario.Perfil = perfil;
                return usuario;
            }, param: new { @Email = email, @Senha = senha = CryptoComponent.Sha256Encrypt(senha)},
            splitOn: "PERFILID");

            //retorna user ou null
            return result.FirstOrDefault();
        }
        catch (Exception)
        {
            throw new ApplicationException($"Falha ao obter usuário.");
        }
    }

    public async Task<Guid?> Inserir(Usuario usuario)
    {
        //connectionString
        using var connection = new SqlConnection(connectionString);

        //capturando o id do usuário
        usuario.PerfilId = await connection.QueryFirstOrDefaultAsync<Guid?>(
            "SELECT ID FROM PERFIL WHERE NOME = 'Operador'");

        //Definindo os parâmetros que serão passados para a procedure
        var parameters = new DynamicParameters();
        parameters.Add("@NOME", usuario.Nome);
        parameters.Add("@EMAIL", usuario.Email);
        parameters.Add("@SENHA", CryptoComponent.Sha256Encrypt(usuario.Senha));
        parameters.Add("@PERFILID", usuario.PerfilId);
        parameters.Add("@USUARIOID", dbType: DbType.Guid, direction: ParameterDirection.Output);

        try
        {
            //executando a procedure
            await connection.ExecuteAsync("SP_CRIAR_USUARIO", parameters, commandType: CommandType.StoredProcedure);
            
            //retornando o ID do usuário criado
            return parameters.Get<Guid?>("@USUARIOID");
        }
        catch (SqlException e)
        {
            throw new ApplicationException($"Falha ao criar usuário: {e.Message}");
        }
    }
}
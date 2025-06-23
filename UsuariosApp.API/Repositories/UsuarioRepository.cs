using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using UsuariosApp.API.Entities;

namespace UsuariosApp.API.Repositories;

public class UsuarioRepository(string connectionString)
{
    public async Task<Guid?> Inserir(Usuario usuario)
    {
        //connectionString
        using var connection = new SqlConnection(connectionString);
        var parameters = new DynamicParameters();

        //Definindo os parâmetros que serão passados para a procedure
        parameters.Add("@NOME", usuario.Nome);
        parameters.Add("@EMAIL", usuario.Email);
        parameters.Add("@SENHA", usuario.Senha);
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
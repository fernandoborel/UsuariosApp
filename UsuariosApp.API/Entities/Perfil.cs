namespace UsuariosApp.API.Entities;

public class Perfil
{
    #region Propriedades
    public Guid? Id { get; set; }
    public string? Nome { get; set; }
    #endregion
   
    #region Relacionamentos
    public ICollection<Usuario>? Usuarios { get; set; }
    #endregion
}
using Kiketpropio.Dominio.Entidades;

namespace Kiketpropio.Services.Interfaces
{
    public interface IUsuarioService
    {
        IEnumerable<Usuario> GetAll();
        Usuario? GetOne(int id);
        Usuario Create(Usuario usuarioInput);
        Usuario? Update(Usuario usuarioInput, int id);
        bool Delete(int id);
    }
}

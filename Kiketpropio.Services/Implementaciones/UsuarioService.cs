using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Services.Interfaces;

namespace Kiketpropio.Services.Implementaciones
{
    public class UsuarioService:IUsuarioService
    {
        private readonly List<Usuario> _usuarioList = new()
        {
            new Usuario(1,"1234","string","string","string","pass",DateTime.Now,"User"),
            new Usuario(2,"1234","string","string","string","pass",DateTime.Now,"User")
        };
        private int _nextId = 3;

        private void IncrementNextId()
        {
            _nextId += 1;
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _usuarioList;
        }

        public Usuario? GetOne(int id)
        {
            return _usuarioList.FirstOrDefault(u => u.IdUsuario == id);
        }

        public Usuario Create(Usuario usuarioInput)
        {
            usuarioInput.IdUsuario = _nextId;
            _usuarioList.Add(usuarioInput);
            IncrementNextId();
            return usuarioInput;
        }

        public Usuario? Update(Usuario usuarioInput,int id)
        {
            var userToBeModified = _usuarioList.FirstOrDefault(u => u.IdUsuario == id);
            if (userToBeModified is null) return null;

            _usuarioList.Remove(userToBeModified);
            userToBeModified.Nombre = usuarioInput.Nombre;
            userToBeModified.FechaNacimiento = usuarioInput.FechaNacimiento;
            userToBeModified.DNI = usuarioInput.DNI;
            userToBeModified.Apellido = usuarioInput.Apellido;
            _usuarioList.Add(userToBeModified);

            return usuarioInput;
        }

        public bool Delete(int id)
        {
            var userToBeDeleted = _usuarioList.FirstOrDefault(u => u.IdUsuario == id);
            if (userToBeDeleted is null) return false;

            _usuarioList.Remove(userToBeDeleted);
            return true;
        }
    }
}

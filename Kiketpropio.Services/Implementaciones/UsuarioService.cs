using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Services.Interfaces;

namespace Kiketpropio.Services.Implementaciones
{
    public class UsuarioService:IUsuarioService
    {
        private readonly List<Usuario> _usuarioList = new()
        {
            new Usuario(1,"30125478","Juan Cruz","Petazzi","admin@kicket.com.ar","Admin123",new DateTime(1983,4,12),"Admin"),
            new Usuario(2,"28944120","Marina","Suarez","marina.suarez@kicket.com.ar","Admin123",new DateTime(1981,9,30),"Admin"),
            new Usuario(3,"35678901","Lucas","Fernandez","lucas.fernandez@gmail.com","User123",new DateTime(1990,2,17),"User"),
            new Usuario(4,"38112450","Camila","Ortega","camila.ortega@gmail.com","User123",new DateTime(1994,7,3),"User"),
            new Usuario(5,"40233781","Matias","Gimenez","matias.gimenez@hotmail.com","User123",new DateTime(1997,11,25),"User"),
            new Usuario(6,"42560334","Sofia","Ledesma","sofia.ledesma@gmail.com","User123",new DateTime(2000,5,8),"User"),
            new Usuario(7,"44789215","Tomas","Alvarez","tomas.alvarez@outlook.com","User123",new DateTime(2003,1,19),"User"),
            new Usuario(8,"33450987","Rodrigo","Benitez","rodrigo.benitez@kicket.com.ar","Vend123",new DateTime(1988,6,14),"Vendedor"),
            new Usuario(9,"37220146","Valentina","Rios","valentina.rios@kicket.com.ar","Vend123",new DateTime(1992,10,2),"Vendedor")
        };
        private int _nextId = 10;

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

        public Usuario? GetByEmail(string email)
        {
            return _usuarioList.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
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
            userToBeModified.Email = usuarioInput.Email;
            userToBeModified.Pass = usuarioInput.Pass;
            userToBeModified.Rol = usuarioInput.Rol;
            _usuarioList.Add(userToBeModified);

            return userToBeModified;
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

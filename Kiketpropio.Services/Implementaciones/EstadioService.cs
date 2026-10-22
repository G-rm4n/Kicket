using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Services.Interfaces;

namespace Kiketpropio.Services.Implementaciones
{
    public class EstadioService:IEstadioService
    {
        private readonly List<Estadio> _EstadioList = new()
        {
            new Estadio(1,"Gigante de Arroyito","Avenida Genova",640,"Rosario"),
            new Estadio(2,"Marcelo Bielsa","Pasaje Juan B. Justo",3550,"Rosario"),
            new Estadio(3,"Mas Monumental","Avenida Figueroa Alcorta",7597,"Buenos Aires"),
            new Estadio(4,"La Bombonera","Brandsen",805,"Buenos Aires"),
            new Estadio(5,"Pedro Bidegain","Avenida Perito Moreno",1750,"Buenos Aires"),
            new Estadio(6,"Presidente Peron","Mozart",2500,"Avellaneda"),
            new Estadio(7,"Libertadores de America","Bochini",751,"Avellaneda"),
            new Estadio(8,"Mario Alberto Kempes","Avenida Ramon Carcano",1550,"Cordoba")
        };
        private int _NextId = 9;

        private void IncrementNextId()
        {
            _NextId += 1;
        }

        public IEnumerable<Estadio> GetAll()
        {
            return _EstadioList;
        }

        public Estadio? GetOne(int id)
        {
            return _EstadioList.FirstOrDefault(e => e.IdEstadio == id);
        }

        public Estadio Create(Estadio estadioInput)
        {
            estadioInput.IdEstadio = _NextId;
            _EstadioList.Add(estadioInput);
            IncrementNextId();
            return estadioInput;
        }

        public Estadio? Update (Estadio estadioInput, int id)
        {
            var estadioToBeUpdated = _EstadioList.FirstOrDefault(e => e.IdEstadio == id);
            if (estadioToBeUpdated is null) return null;

            _EstadioList.Remove(estadioToBeUpdated);
            estadioToBeUpdated.Nombre = estadioInput.Nombre;
            estadioToBeUpdated.Calle = estadioInput.Calle;
            estadioToBeUpdated.Nro = estadioInput.Nro;
            estadioToBeUpdated.Ciudad = estadioInput.Ciudad;
            _EstadioList.Add(estadioToBeUpdated);

            return estadioToBeUpdated;
        }

        public bool Delete(int id)
        {
            var estadioToBeDeleted = _EstadioList.FirstOrDefault(e => e.IdEstadio == id);
            if (estadioToBeDeleted is null) return false;

            _EstadioList.Remove(estadioToBeDeleted);
            return true;
        }
    }
}

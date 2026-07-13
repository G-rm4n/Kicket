using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Services.Interfaces;

namespace Kiketpropio.Services.Implementaciones
{
    public class EstadioService:IEstadioService
    {
        private readonly List<Estadio> _EstadioList = new()
        {
            new Estadio(1,"Estadio1","Calle1",3,"Rosario"),
            new Estadio(2,"Estadio2","Calle2",3,"Rosario")
        };
        private int _NextId = 3;

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

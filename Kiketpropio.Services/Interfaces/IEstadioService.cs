using Kiketpropio.Dominio.Entidades;

namespace Kiketpropio.Services.Interfaces
{
    public interface IEstadioService
    {
        IEnumerable<Estadio> GetAll();
        Estadio? GetOne(int id);
        Estadio Create(Estadio estadioInput);
        Estadio? Update(Estadio estadioInput, int id);
        bool Delete(int id);
    }
}

using Kiketpropio.Dominio.Entidades;

namespace Kiketpropio.Services.Interfaces
{
    public interface IClubService
    {
        IEnumerable<Club> GetAll();
        Club? GetOne(int id);
        Club Create(Club clubInput);
        Club? Update(Club clubInput, int id);
        bool Delete(int id);
    }
}

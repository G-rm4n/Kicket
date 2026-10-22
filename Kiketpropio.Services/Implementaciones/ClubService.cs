using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Services.Interfaces;

namespace Kiketpropio.Services.Implementaciones
{
    public class ClubService:IClubService
    {
        private readonly List<Club> _clubList = new()
        {
            new Club(1,"Rosario Central","https://kicket.com.ar/logos/rosario-central.png"),
            new Club(2,"Newell's Old Boys","https://kicket.com.ar/logos/newells.png"),
            new Club(3,"River Plate","https://kicket.com.ar/logos/river-plate.png"),
            new Club(4,"Boca Juniors","https://kicket.com.ar/logos/boca-juniors.png"),
            new Club(5,"Racing Club","https://kicket.com.ar/logos/racing-club.png"),
            new Club(6,"Independiente","https://kicket.com.ar/logos/independiente.png"),
            new Club(7,"San Lorenzo","https://kicket.com.ar/logos/san-lorenzo.png"),
            new Club(8,"Talleres","https://kicket.com.ar/logos/talleres.png"),
        };
        private int _NextId = 9;

        private void IncrementNextId()
        {
            _NextId += 1;
        }

        public IEnumerable<Club> GetAll()
        {
            return _clubList;
        }

        public Club? GetOne(int id)
        {
            return _clubList.FirstOrDefault(c => c.IdClub == id);
        }

        public Club Create(Club clubInput)
        {
            clubInput.IdClub = _NextId;
            _clubList.Add(clubInput);
            IncrementNextId();
            return clubInput;
        }

        public Club? Update(Club clubInput,int id)
        {
            var clubFounded = _clubList.FirstOrDefault(c => c.IdClub == id);
            if (clubFounded is null) return null;

            _clubList.Remove(clubFounded);
            clubFounded.Nombre = clubInput.Nombre;
            clubFounded.Logo_Url = clubInput.Logo_Url;
            _clubList.Add(clubFounded);

            return clubFounded;
        }

        public bool Delete(int id)
        {
            var clubToBeDeleted = _clubList.FirstOrDefault(c => c.IdClub == id);
            if (clubToBeDeleted is null) return false;

            _clubList.Remove(clubToBeDeleted);
            return true;
        }
    }
}

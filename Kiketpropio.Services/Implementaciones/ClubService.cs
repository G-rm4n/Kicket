using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Services.Interfaces;

namespace Kiketpropio.Services.Implementaciones
{
    public class ClubService:IClubService
    {
        private readonly List<Club> _clubList = new()
        {
            new Club(1,"Atletico","URL2"),
            new Club(2,"Racing","URL1"),
        };
        private int _NextId = 3;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<(string Token, DateTime ExpiraEn, Usuario Usuario)?> Login(string mail, string pass);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet_Rpg.Models;

namespace DotNet_Rpg.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(UserData user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);

    }
}
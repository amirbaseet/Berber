using Berber.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Interfaces
{
    public interface IUserManager
    {
        User Login(string name);
        void Register(User user);
        User GetUserById(int id);
    }
}

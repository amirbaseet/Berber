using Berber.Core.Interfaces;
using Berber.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Managers
{
    public class UserManager : IUserManager
    {
        private readonly List<User> _users;

        public UserManager(List<User> users)
        {
            _users = users;
        }

        public void Register(User user)
        {
            _users.Add(user);
        }

        public User Login(string name)
        {
            return _users.FirstOrDefault(u => u.Name == name);
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }
    }
}

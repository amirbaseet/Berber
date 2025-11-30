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
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public void Register(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (_users.Any(u => u.Id == user.Id))
                throw new InvalidOperationException($"User with ID {user.Id} already exists.");

            _users.Add(user);
        }

        public User Login(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            return _users.FirstOrDefault(u => u.Name == name);
        }

        public User GetUserById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be positive.", nameof(id));

            return _users.FirstOrDefault(u => u.Id == id);
        }
    }
}

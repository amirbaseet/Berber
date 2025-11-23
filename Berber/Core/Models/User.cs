using Berber.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; protected set; }

        protected User(int id, string name, UserRole role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public override string ToString()
        {
            return $"{Name} ({Role})";
        }
    }
}

using Berber.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Models
{
    public class Admin : User
    {
        public Admin(int id, string name)
            : base(id, name, UserRole.Admin)
        {
        }
    }
}

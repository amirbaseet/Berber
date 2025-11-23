using Berber.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Models
{
    public class Customer : User
    {
        public Customer(int id, string name)
            : base(id, name, UserRole.Customer)
        {
        }
    }
}

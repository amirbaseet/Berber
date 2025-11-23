using Berber.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Models
{
    public class Employee : User
    {
        public List<Service> ServicesCanDo { get; set; } = new List<Service>();
        public List<TimeRange> Availability { get; set; } = new List<TimeRange>();
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public Employee(int id, string name)
            : base(id, name, UserRole.Employee)
        {
        }

        public bool CanPerform(Service service)
        {
            return ServicesCanDo.Contains(service);
        }
    }
}

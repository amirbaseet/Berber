using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        // Working hours per day (e.g., Monday → 09:00-18:00)
        public Dictionary<DayOfWeek, TimeRange> WorkingHours { get; set; }
            = new Dictionary<DayOfWeek, TimeRange>();

        public List<Service> Services { get; set; }
            = new List<Service>();

        public List<Employee> Employees { get; set; }
            = new List<Employee>();

        public Salon(int id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;
        }

        public void AddService(Service service)
        {
            Services.Add(service);
        }

        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
        }

        public bool IsOpenAt(DateTime dateTime)
        {
            if (!WorkingHours.ContainsKey(dateTime.DayOfWeek))
                return false;

            TimeRange range = WorkingHours[dateTime.DayOfWeek];

            TimeSpan t = dateTime.TimeOfDay;
            return t >= range.Start && t <= range.End;

        }

        public override string ToString()
        {
            return $"{Name} - {Address}";
        }
    }

}

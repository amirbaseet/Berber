using Berber.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Data
{
    public static class Database
    {
        public static List<User> Users = new List<User>();
        public static List<Salon> Salons = new List<Salon>();
        public static List<Service> Services = new List<Service>();
        public static List<Employee> Employees = new List<Employee>();
        public static List<Appointment> Appointments = new List<Appointment>();
    }
}

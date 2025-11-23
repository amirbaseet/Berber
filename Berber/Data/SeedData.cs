using Berber.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Data
{
    public static class SeedData
    {
        public static void Load()
        {
            // --- Create Admin ---
            Admin admin = new Admin(1, "AdminUser");
            Database.Users.Add(admin);

            // --- Create Customers ---
            Customer c1 = new Customer(2, "John");
            Customer c2 = new Customer(3, "Sarah");

            Database.Users.Add(c1);
            Database.Users.Add(c2);

            // --- Create Salon ---
            Salon salon = new Salon(1, "Golden Scissors", "Main Street 45");

            // Working hours: Monday–Friday 09:00 - 18:00
            for (int i = 1; i <= 5; i++)
            {
                DateTime start = DateTime.Today.AddDays(i).Date.AddHours(9);
                DateTime end = DateTime.Today.AddDays(i).Date.AddHours(18);

                salon.WorkingHours[(DayOfWeek)i] =
                    new TimeRange(start, end);
            }

            Database.Salons.Add(salon);

            // --- Create Services ---
            Service haircut = new Service(1, "Haircut", 30, 20m);
            Service beard = new Service(2, "Beard Trim", 20, 10m);
            Service color = new Service(3, "Hair Coloring", 60, 50m);

            Database.Services.Add(haircut);
            Database.Services.Add(beard);
            Database.Services.Add(color);

            // Add services to salon
            salon.Services.Add(haircut);
            salon.Services.Add(beard);
            salon.Services.Add(color);

            // --- Create Employees ---
            Employee e1 = new Employee(10, "Mike");
            Employee e2 = new Employee(11, "Alex");

            // Assign skills
            e1.ServicesCanDo.Add(haircut);
            e1.ServicesCanDo.Add(beard);

            e2.ServicesCanDo.Add(haircut);
            e2.ServicesCanDo.Add(color);

            // Add employees to salon
            salon.Employees.Add(e1);
            salon.Employees.Add(e2);

            // Add availability
            e1.Availability.Add(new TimeRange(
                DateTime.Today.AddHours(9),
                DateTime.Today.AddHours(17)
            ));

            e2.Availability.Add(new TimeRange(
                DateTime.Today.AddHours(10),
                DateTime.Today.AddHours(18)
            ));

            Database.Employees.Add(e1);
            Database.Employees.Add(e2);

            // ✅ REQUIRED FOR LOGIN
            Database.Users.Add(e1);
            Database.Users.Add(e2);
        }
    }

}

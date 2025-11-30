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

            // Working hours: Monday–Friday 09:00 - 18:00 (time-only)
            TimeSpan open = new TimeSpan(9, 0, 0);
            TimeSpan close = new TimeSpan(18, 0, 0);

            salon.WorkingHours[DayOfWeek.Monday] = new TimeRange(open, close);
            salon.WorkingHours[DayOfWeek.Tuesday] = new TimeRange(open, close);
            salon.WorkingHours[DayOfWeek.Wednesday] = new TimeRange(open, close);
            salon.WorkingHours[DayOfWeek.Thursday] = new TimeRange(open, close);
            salon.WorkingHours[DayOfWeek.Friday] = new TimeRange(open, close);

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
            Employee e3 = new Employee(12, "x");

            // Assign service skills
            e1.ServicesCanDo.Add(haircut);
            //testttt
            e1.ServicesCanDo.Add(beard);

            e2.ServicesCanDo.Add(haircut);
            e2.ServicesCanDo.Add(color);

            // Add employees to salon
            salon.Employees.Add(e1);
            salon.Employees.Add(e2);

            // --- Employee Availability (Time-only, no specific day)
            // They will be available every day the customer tries booking,
            // but booking will compare weekday & salon working hours.

            // Mike: 09:00 - 17:00
            e1.Availability.Add(new TimeRange(
                new TimeSpan(9, 0, 0),
                new TimeSpan(17, 0, 0)
            ));

            // Alex: 10:00 - 18:00
            e2.Availability.Add(new TimeRange(
                new TimeSpan(10, 0, 0),
                new TimeSpan(18, 0, 0)
            ));
            // Add to database lists
            Database.Employees.Add(e1);
            Database.Employees.Add(e2);

            // Add employees to users (required for login)
            Database.Users.Add(e1);
            Database.Users.Add(e2);
        }


    }

}

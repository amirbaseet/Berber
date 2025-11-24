using Berber.Core.Models;
using Berber.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.UI.MenuSystem
{
    public class EmployeeScheduleMenu
    {
        private readonly Employee _employee;

        public EmployeeScheduleMenu(Employee employee)
        {
            _employee = employee;
        }

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Weekly Schedule");

                Console.WriteLine("1. View Weekly Summary");
                Console.WriteLine("2. View Detailed Day Schedule");
                Console.WriteLine("3. Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1: ShowWeeklySummary(); break;
                    case 2: ShowDayDetails(); break;
                    case 3: return;
                    default:
                        ConsoleUIHelper.PrintError("Invalid option.");
                        break;
                }
            }
        }

        private void ShowWeeklySummary()
        {
            ConsoleUIHelper.Title("Weekly Summary");

            // Get Monday as start of week
            DateTime today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime weekStart = today.AddDays(-diff);

            for (int i = 0; i < 7; i++)
            {
                DateTime day = weekStart.AddDays(i);

                int count = _employee.Appointments
                    .Where(a => a.StartTime.Date == day.Date)
                    .Count();

                string label = $"{day:ddd yyyy-MM-dd} : {count} appointments";

                if (day.Date == today.Date)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(label + "  ← Today");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(label);
                }
            }

            ConsoleUIHelper.Pause();
        }

        private void ShowDayDetails()
        {
            ConsoleUIHelper.Title("Day Schedule Details");

            // Ask for date
            DateTime day = InputHelper.ReadDateTime("Enter date (yyyy-MM-dd): ").Date;

            // Filter appointments by date
            List<Appointment> appointments = _employee.Appointments
                .Where(a => a.StartTime.Date == day)
                .OrderBy(a => a.StartTime)
                .ToList();

            if (appointments.Count == 0)
            {
                ConsoleUIHelper.PrintError("No appointments for this day.");
                ConsoleUIHelper.Pause();
                return;
            }

            ConsoleUIHelper.Line();
            Console.WriteLine($"Schedule for {day:dddd, yyyy-MM-dd}");
            ConsoleUIHelper.Line();

            foreach (Appointment a in appointments)
            {
                Console.WriteLine(
                    $"{a.StartTime:HH:mm} - {a.EndTime:HH:mm} | " +
                    $"{a.Service.Name} | Customer: {a.Customer.Name} | Status: {a.Status}"
                );
            }

            ConsoleUIHelper.Pause();
        }

    }
}

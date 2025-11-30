using Berber.Core.Models;
using Berber.Data;
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
                    case 2: ShowDetailedDaySchedule(); break;
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

                int count = Database.Appointments
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

        private void ShowDetailedDaySchedule()
        {
            ConsoleUIHelper.Title("Select a Day");

            DateTime today = DateTime.Today;
            List<DateTime> next7Days = new List<DateTime>();

            for (int i = 0; i < 7; i++)
                next7Days.Add(today.AddDays(i));

            // Display the 7-day menu
            for (int i = 0; i < next7Days.Count; i++)
            {
                DateTime d = next7Days[i];
                Console.WriteLine($"{i + 1}. {d:ddd yyyy-MM-dd}");
            }

            int choice = InputHelper.ReadInt("Choose day: ");

            if (choice < 1 || choice > 7)
            {
                ConsoleUIHelper.PrintError("Invalid selection.");
                ConsoleUIHelper.Pause();
                return;
            }

            DateTime selectedDay = next7Days[choice - 1];

            // Filter employee appointments for that day
            var list = Database.Appointments
                .Where(a => a.Employee.Id == _employee.Id &&
                            a.StartTime.Date == selectedDay.Date)
                .OrderBy(a => a.StartTime)
                .ToList();

            ConsoleUIHelper.Title($"Schedule for {selectedDay:dddd, yyyy-MM-dd}");

            if (list.Count == 0)
            {
                ConsoleUIHelper.PrintError("No appointments for this day.");
                ConsoleUIHelper.Pause();
                return;
            }

            foreach (var appt in list)
            {
                Console.WriteLine(
                    $"{appt.StartTime:HH:mm} - {appt.EndTime:HH:mm} | " +
                    $"{appt.Service.Name} | Customer: {appt.Customer.Name} | Status: {appt.Status}"
                );
            }

            ConsoleUIHelper.Pause();
        }
    }
}

using Berber.Core.Models;
using Berber.Core.Utils;
using Berber.Data;
using Berber.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.UI.MenuSystem
{
    public class EmployeeMenu
    {
        private readonly Employee _employee;

        public EmployeeMenu(Employee employee)
        {
            _employee = employee;
        }

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Weekly Schedule");

                ConsoleUIHelper.PrintOption(1, "View Weekly Summary");
                ConsoleUIHelper.PrintOption(2, "View Detailed Day Schedule");
                ConsoleUIHelper.PrintOption(3, "View My Appointments (full list)");
                ConsoleUIHelper.PrintOption(4, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1:
                        ShowWeeklySummary();
                        break;
                    case 2:
                        ShowDetailedDaySchedule();
                        break;
                    case 3:
                        ShowAllAppointments();
                        break;
                    case 4:
                        return;

                    default:
                        ConsoleUIHelper.PrintError("Invalid option.");
                        break;
                }
            }
        }

        // =====================
        //  WEEKLY SUMMARY
        // =====================

        private void ShowWeeklySummary()
        {
            ConsoleUIHelper.Title("Weekly Summary");

            // Show the next 7 days (just like customer booking)
            DateTime start = DateTime.Today;

            for (int i = 0; i < 7; i++)
            {
                DateTime day = start.AddDays(i);

                int count = Database.Appointments
                    .Where(a => a.Employee.Id == _employee.Id &&
                                a.StartTime.Date == day.Date)
                    .Count();

                Console.WriteLine($"{day:ddd yyyy-MM-dd} - {count} appointment(s)");
            }

            ConsoleUIHelper.Pause();
        }

        // =====================
        //  VIEW DETAIL OF DAY
        // =====================

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
        // =====================
        //  FULL APPOINTMENT LIST
        // =====================

        private void ShowAllAppointments()
        {
            ConsoleUIHelper.Title("All My Appointments");

            var list = Database.Appointments
                .Where(a => a.Employee.Id == _employee.Id)
                .OrderBy(a => a.StartTime)
                .ToList();

            if (list.Count == 0)
            {
                ConsoleUIHelper.PrintError("You have no appointments.");
                ConsoleUIHelper.Pause();
                return;
            }

            foreach (var appt in list)
            {
                Console.WriteLine(
                    $"#{appt.Id} | {appt.StartTime:yyyy-MM-dd HH:mm} | " +
                    $"{appt.Service.Name} | Customer: {appt.Customer.Name} | {appt.Status}"
                );
            }

            ConsoleUIHelper.Pause();
        }
    }
}

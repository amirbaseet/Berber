using Berber.Core.Models;
using Berber.Core.Utils;
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

            List<DateTime> days = CalendarHelper.GetNext7Days();

            foreach (var day in days)
            {
                int count = _employee.Appointments
                    .Count(a => a.StartTime.Date == day.Date);

                Console.WriteLine($"{day:ddd yyyy-MM-dd} - {count} appointment(s)");
            }

            ConsoleUIHelper.Pause();
        }

        // =====================
        //  VIEW DETAIL OF DAY
        // =====================

        private void ShowDetailedDaySchedule()
        {
            DateTime selectedDay =
                CalendarHelper.SelectDayFromNext7Days("Select a Day");

            if (selectedDay == DateTime.MinValue)
            {
                ConsoleUIHelper.PrintError("Invalid day selection.");
                ConsoleUIHelper.Pause();
                return;
            }

            ConsoleUIHelper.Title($"Schedule for {selectedDay:ddd yyyy-MM-dd}");

            var list = _employee.Appointments
                .Where(a => a.StartTime.Date == selectedDay.Date)
                .OrderBy(a => a.StartTime)
                .ToList();

            if (list.Count == 0)
            {
                ConsoleUIHelper.PrintError("No appointments for this day.");
                ConsoleUIHelper.Pause();
                return;
            }

            ConsoleUIHelper.Line();

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

            var list = _employee.Appointments
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

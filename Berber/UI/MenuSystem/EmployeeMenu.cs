using Berber.Core.Models;
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
                ConsoleUIHelper.Title("Employee Menu");

                Console.WriteLine("Logged in as: " + _employee.Name);
                ConsoleUIHelper.Line();

                ConsoleUIHelper.PrintOption(1, "View My Appointments");
                ConsoleUIHelper.PrintOption(2, "Add Availability");
                ConsoleUIHelper.PrintOption(3, "View Weekly Schedule"); 
                ConsoleUIHelper.PrintOption(4, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1: ViewAppointments(); break;
                    case 2: AddAvailability(); break;
                    case 3: new EmployeeScheduleMenu(_employee).Show(); break;  // NEW
                    case 4: return;
                    default:
                        ConsoleUIHelper.PrintError("Invalid option.");
                        break;
                }
            }
        }

        private void ViewAppointments()
        {
            ConsoleUIHelper.Title("My Appointments");

            if (_employee.Appointments.Count == 0)
            {
                ConsoleUIHelper.PrintError("No appointments.");
            }
            else
            {
                foreach (Appointment a in _employee.Appointments)
                {
                    Console.WriteLine(a.ToString());
                }
            }

            ConsoleUIHelper.Pause();
        }

        private void AddAvailability()
        {
            ConsoleUIHelper.Title("Add Availability");

            DateTime start = InputHelper.ReadDateTime("Start time (yyyy-MM-dd HH:mm): ");
            DateTime end = InputHelper.ReadDateTime("End time (yyyy-MM-dd HH:mm): ");

            try
            {
                _employee.Availability.Add(new TimeRange(start, end));
                ConsoleUIHelper.PrintSuccess("Availability added.");
            }
            catch (Exception ex)
            {
                ConsoleUIHelper.PrintError(ex.Message);
            }

            ConsoleUIHelper.Pause();
        }
    }

}

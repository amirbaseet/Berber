using Berber.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.UI.MenuSystem
{
    public class AdminMenu
    {
        private readonly SalonManagementMenu _salonMenu = new SalonManagementMenu();
        private readonly EmployeeManagementMenu _employeeMenu = new EmployeeManagementMenu();

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Admin Menu");

                ConsoleUIHelper.PrintOption(1, "Salon Management");
                ConsoleUIHelper.PrintOption(2, "Employee Management");
                ConsoleUIHelper.PrintOption(3, "Service Management");
                ConsoleUIHelper.PrintOption(4, "Appointment Management");
                ConsoleUIHelper.PrintOption(5, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1:
                        _salonMenu.Show();
                        break;

                    case 2:
                        _employeeMenu.Show();
                        break;

                    case 3:
                        new ServiceManagementMenu().Show();
                        break;

                    case 4:
                        new AppointmentManagementMenu().Show();
                        break;

                    case 5:
                        return;

                    default:
                        ConsoleUIHelper.PrintError("Invalid choice.");
                        break;
                }
            }
        }
    }
}



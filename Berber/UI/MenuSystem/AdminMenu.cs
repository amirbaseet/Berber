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

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Admin Menu");

                ConsoleUIHelper.PrintOption(1, "Salon Management");
                ConsoleUIHelper.PrintOption(2, "Employee Management (coming soon)");
                ConsoleUIHelper.PrintOption(3, "Service Management (coming soon)");
                ConsoleUIHelper.PrintOption(4, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1:
                        _salonMenu.Show();
                        break;

                    case 4:
                        return;

                    default:
                        ConsoleUIHelper.PrintError("Invalid choice.");
                        break;
                }
            }
        }
    }
}



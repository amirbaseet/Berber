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
        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Admin Menu");

                ConsoleUIHelper.PrintOption(1, "Manage Salons");
                ConsoleUIHelper.PrintOption(2, "Manage Employees");
                ConsoleUIHelper.PrintOption(3, "Manage Services");
                ConsoleUIHelper.PrintOption(4, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1:
                        ConsoleUIHelper.PrintSuccess("Salon management coming soon...");
                        ConsoleUIHelper.Pause();
                        break;

                    case 2:
                        ConsoleUIHelper.PrintSuccess("Employee management coming soon...");
                        ConsoleUIHelper.Pause();
                        break;

                    case 3:
                        ConsoleUIHelper.PrintSuccess("Service management coming soon...");
                        ConsoleUIHelper.Pause();
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

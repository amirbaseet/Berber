using Berber.Core.Interfaces;
using Berber.Core.Models.Enums;
using Berber.Core.Models;
using Berber.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.UI.MenuSystem
{
    public class MainMenu
    {
        private readonly IUserManager _userManager;

        public MainMenu(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Barber Management System");

                ConsoleUIHelper.PrintOption(1, "Login");
                ConsoleUIHelper.PrintOption(2, "Exit");

                int choice = InputHelper.ReadInt("Choose an option: ");

                switch (choice)
                {
                    case 1:
                        Login();
                        break;

                    case 2:
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        ConsoleUIHelper.PrintError("Invalid option.");
                        break;
                }
            }
        }

        private void Login()
        {
            string username = InputHelper.ReadString("Enter name: ");

            User user = _userManager.Login(username);

            if (user == null)
            {
                ConsoleUIHelper.PrintError("User not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            switch (user.Role)
            {
                case UserRole.Admin:
                    new AdminMenu().Show();
                    break;

                case UserRole.Employee:
                    new EmployeeMenu((Employee)user).Show();
                    break;

                case UserRole.Customer:
                    new CustomerMenu((Customer)user).Show();
                    break;
            }
        }
    }

}

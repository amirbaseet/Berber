using Berber.Core.Interfaces;
using Berber.Core.Managers;
using Berber.Data;
using Berber.UI.MenuSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load seed data
            SeedData.Load();

            // Initialize managers
            IUserManager userManager = new UserManager(Database.Users);

            // Start main menu
            MainMenu mainMenu = new MainMenu(userManager);
            mainMenu.Show();

            Console.WriteLine("hello World!");
        }
    }
}

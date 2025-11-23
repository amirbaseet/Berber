using Berber.Core.Interfaces;
using Berber.Core.Managers;
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
    public class ServiceManagementMenu
    {
        private readonly ISalonManager _salonManager;

        public ServiceManagementMenu()
        {
            _salonManager = new SalonManager(Database.Salons);
        }

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Service Management");

                ConsoleUIHelper.PrintOption(1, "View All Services");
                ConsoleUIHelper.PrintOption(2, "Add New Service");
                ConsoleUIHelper.PrintOption(3, "Edit Service Price");
                ConsoleUIHelper.PrintOption(4, "Assign Service to Salon");
                ConsoleUIHelper.PrintOption(5, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1: ViewAllServices(); break;
                    case 2: AddService(); break;
                    case 3: EditServicePrice(); break;
                    case 4: AssignServiceToSalon(); break;
                    case 5: return;

                    default:
                        ConsoleUIHelper.PrintError("Invalid choice.");
                        break;
                }
            }
        }

        private void ViewAllServices()
        {
            ConsoleUIHelper.Title("Service List");

            if (Database.Services.Count == 0)
            {
                ConsoleUIHelper.PrintError("No services found.");
            }
            else
            {
                foreach (Service s in Database.Services)
                {
                    Console.WriteLine(
                        $"ID: {s.Id} | {s.Name} | {s.DurationMinutes} minutes | {s.Price:C}");
                }
            }

            ConsoleUIHelper.Pause();
        }

        private void AddService()
        {
            ConsoleUIHelper.Title("Add New Service");

            int id = InputHelper.ReadInt("Service ID: ");
            string name = InputHelper.ReadString("Service Name: ");
            int duration = InputHelper.ReadInt("Duration in minutes: ");
            decimal price = InputHelper.ReadDecimal("Price: ");

            Service service = new Service(id, name, duration, price);

            Database.Services.Add(service);

            ConsoleUIHelper.PrintSuccess("Service added successfully.");
            ConsoleUIHelper.Pause();
        }

        private void EditServicePrice()
        {
            ConsoleUIHelper.Title("Edit Service Price");

            int id = InputHelper.ReadInt("Enter Service ID: ");
            Service service = Database.Services.Find(s => s.Id == id);

            if (service == null)
            {
                ConsoleUIHelper.PrintError("Service not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            decimal newPrice = InputHelper.ReadDecimal("Enter new price: ");
            service.Price = newPrice;

            ConsoleUIHelper.PrintSuccess("Price updated successfully.");
            ConsoleUIHelper.Pause();
        }

        private void AssignServiceToSalon()
        {
            ConsoleUIHelper.Title("Assign Service to Salon");

            // Choose service
            foreach (Service s in Database.Services)
            {
                Console.WriteLine($"{s.Id} - {s.Name}");
            }

            int serviceId = InputHelper.ReadInt("Service ID: ");
            Service serviceToAssign = Database.Services.Find(s => s.Id == serviceId);

            if (serviceToAssign == null)
            {
                ConsoleUIHelper.PrintError("Service not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Choose salon
            foreach (Salon s in Database.Salons)
            {
                Console.WriteLine($"{s.Id} - {s.Name}");
            }

            int salonId = InputHelper.ReadInt("Salon ID: ");
            Salon salon = _salonManager.GetSalonById(salonId);

            if (salon == null)
            {
                ConsoleUIHelper.PrintError("Salon not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            salon.Services.Add(serviceToAssign);

            ConsoleUIHelper.PrintSuccess("Service assigned to salon.");
            ConsoleUIHelper.Pause();
        }
    }

}

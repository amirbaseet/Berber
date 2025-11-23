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
    public class SalonManagementMenu
    {
        private readonly ISalonManager _salonManager;

        public SalonManagementMenu()
        {
            _salonManager = new SalonManager(Database.Salons);
        }

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Salon Management");

                ConsoleUIHelper.PrintOption(1, "View All Salons");
                ConsoleUIHelper.PrintOption(2, "Add New Salon");
                ConsoleUIHelper.PrintOption(3, "Set Working Hours");
                ConsoleUIHelper.PrintOption(4, "Edit Salon Name");
                ConsoleUIHelper.PrintOption(5, "Edit Salon Address");
                ConsoleUIHelper.PrintOption(6, "Delete Salon");
                ConsoleUIHelper.PrintOption(7, "Back");


                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1: ViewSalons(); break;
                    case 2: AddSalon(); break;
                    case 3: SetWorkingHours(); break;
                    case 4: EditSalonName(); break;
                    case 5: EditSalonAddress(); break;
                    case 6: DeleteSalon(); break;
                    case 7: return;

                    default:
                        ConsoleUIHelper.PrintError("Invalid option.");
                        break;
                }
            }
        }

        private void ViewSalons()
        {
            ConsoleUIHelper.Title("Salon List");

            if (Database.Salons.Count == 0)
            {
                ConsoleUIHelper.PrintError("No salons found.");
            }
            else
            {
                foreach (Salon s in Database.Salons)
                {
                    Console.WriteLine($"ID: {s.Id} | {s.Name} | {s.Address}");
                }
            }

            ConsoleUIHelper.Pause();
        }

        private void AddSalon()
        {
            ConsoleUIHelper.Title("Add New Salon");

            int id = InputHelper.ReadInt("Salon ID: ");
            string name = InputHelper.ReadString("Salon Name: ");
            string address = InputHelper.ReadString("Address: ");

            Salon salon = new Salon(id, name, address);

            _salonManager.AddSalon(salon);

            ConsoleUIHelper.PrintSuccess("Salon added successfully.");
            ConsoleUIHelper.Pause();
        }

        private void SetWorkingHours()
        {
            ConsoleUIHelper.Title("Set Working Hours");

            int id = InputHelper.ReadInt("Enter Salon ID: ");
            Salon salon = _salonManager.GetSalonById(id);

            if (salon == null)
            {
                ConsoleUIHelper.PrintError("Salon not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            Console.WriteLine("Choose day of week:");
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                Console.WriteLine($"{(int)day} - {day}");
            }

            int dayChoice = InputHelper.ReadInt("Day: ");
            DayOfWeek chosenDay = (DayOfWeek)dayChoice;

            DateTime start = InputHelper.ReadDateTime("Start Time (yyyy-MM-dd HH:mm): ");
            DateTime end = InputHelper.ReadDateTime("End Time (yyyy-MM-dd HH:mm): ");

            try
            {
                salon.WorkingHours[chosenDay] = new TimeRange(start, end);
                ConsoleUIHelper.PrintSuccess("Working hours updated.");
            }
            catch (Exception ex)
            {
                ConsoleUIHelper.PrintError(ex.Message);
            }

            ConsoleUIHelper.Pause();
        }
        private void EditSalonName()
        {
            ConsoleUIHelper.Title("Edit Salon Name");

            int id = InputHelper.ReadInt("Enter Salon ID: ");
            Salon salon = _salonManager.GetSalonById(id);

            if (salon == null)
            {
                ConsoleUIHelper.PrintError("Salon not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            string newName = InputHelper.ReadString("Enter new salon name: ");
            salon.Name = newName;

            ConsoleUIHelper.PrintSuccess("Salon name updated successfully.");
            ConsoleUIHelper.Pause();
        }

        private void EditSalonAddress()
        {
            ConsoleUIHelper.Title("Edit Salon Address");

            int id = InputHelper.ReadInt("Enter Salon ID: ");
            Salon salon = _salonManager.GetSalonById(id);

            if (salon == null)
            {
                ConsoleUIHelper.PrintError("Salon not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            string newAddress = InputHelper.ReadString("Enter new address: ");
            salon.Address = newAddress;

            ConsoleUIHelper.PrintSuccess("Salon address updated.");
            ConsoleUIHelper.Pause();
        }

        private void DeleteSalon()
        {
            ConsoleUIHelper.Title("Delete Salon");

            int id = InputHelper.ReadInt("Enter Salon ID: ");
            Salon salon = _salonManager.GetSalonById(id);

            if (salon == null)
            {
                ConsoleUIHelper.PrintError("Salon not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Check employees
            if (salon.Employees.Count > 0)
            {
                ConsoleUIHelper.PrintError("Cannot delete salon: it has employees assigned.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Check appointments
            foreach (Appointment a in Database.Appointments)
            {
                if (a.Salon == salon)
                {
                    ConsoleUIHelper.PrintError("Cannot delete salon: it has appointments in the system.");
                    ConsoleUIHelper.Pause();
                    return;
                }
            }

            // Safe to delete
            Database.Salons.Remove(salon);

            ConsoleUIHelper.PrintSuccess("Salon deleted successfully.");
            ConsoleUIHelper.Pause();
        }

    }


}

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
            ConsoleUIHelper.Title("Set Salon Working Hours");

            int id = InputHelper.ReadInt("Enter Salon ID: ");
            Salon salon = _salonManager.GetSalonById(id);

            if (salon == null)
            {
                ConsoleUIHelper.PrintError("Salon not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            Console.WriteLine($"\nEditing working hours for: {salon.Name}");

            ConsoleUIHelper.Line();
            Console.WriteLine("1. Set SAME working hours for all days");
            Console.WriteLine("2. Set working hours for EACH day separately");
            Console.WriteLine("3. View current working hours");
            int choice = InputHelper.ReadInt("Choose: ");

            switch (choice)
            {
                case 1:
                    SetSameHoursForAllDays(salon);
                    break;

                case 2:
                    SetHoursForEachDay(salon);
                    break;

                case 3:
                    ViewSalonWorkingHours(salon);
                    break;

                default:
                    ConsoleUIHelper.PrintError("Invalid option.");
                    break;
            }

            ConsoleUIHelper.Pause();
        }
        private void SetSameHoursForAllDays(Salon salon)
        {
            ConsoleUIHelper.Title("Set Same Working Hours");

            int startHour = InputHelper.ReadInt("Start hour (0–23): ");
            int startMinute = InputHelper.ReadInt("Start minute (0–59): ");
            int endHour = InputHelper.ReadInt("End hour (0–23): ");
            int endMinute = InputHelper.ReadInt("End minute (0–59): ");

            if (endHour < startHour || (endHour == startHour && endMinute <= startMinute))
            {
                ConsoleUIHelper.PrintError("End time must be after start time.");
                return;
            }

            DateTime open = new DateTime(1, 1, 1, startHour, startMinute, 0);
            DateTime close = new DateTime(1, 1, 1, endHour, endMinute, 0);

            salon.WorkingHours.Clear();

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                salon.WorkingHours[day] = new TimeRange(open, close);

            ConsoleUIHelper.PrintSuccess("Working hours set for all days!");
        }

        private void SetHoursForEachDay(Salon salon)
        {
            ConsoleUIHelper.Title("Set Working Hours Per Day");

            salon.WorkingHours.Clear();

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                Console.WriteLine($"\n--- {day} ---");
                Console.WriteLine("Leave blank to set this day as CLOSED.");

                string startInput = InputHelper.ReadString("Start hour (empty = closed): ");

                if (string.IsNullOrWhiteSpace(startInput))
                {
                    // Day closed
                    continue;
                }

                int startHour = int.Parse(startInput);
                int startMinute = InputHelper.ReadInt("Start minute (0–59): ");
                int endHour = InputHelper.ReadInt("End hour (0–23): ");
                int endMinute = InputHelper.ReadInt("End minute (0–59): ");

                if (endHour < startHour || (endHour == startHour && endMinute <= startMinute))
                {
                    ConsoleUIHelper.PrintError("Invalid time range. Skipping this day.");
                    continue;
                }

                DateTime open = new DateTime(1, 1, 1, startHour, startMinute, 0);
                DateTime close = new DateTime(1, 1, 1, endHour, endMinute, 0);

                salon.WorkingHours[day] = new TimeRange(open, close);
            }

            ConsoleUIHelper.PrintSuccess("Working hours updated!");
        }

        private void ViewSalonWorkingHours(Salon salon)
        {
            ConsoleUIHelper.Title("Current Working Hours");

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                if (!salon.WorkingHours.ContainsKey(day))
                {
                    Console.WriteLine($"{day}: CLOSED");
                }
                else
                {
                    var range = salon.WorkingHours[day];
                    Console.WriteLine($"{day}: {range.Start:HH:mm} - {range.End:HH:mm}");
                }
            }
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

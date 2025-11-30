using Berber.Core.Interfaces;
using Berber.Core.Managers;
using Berber.Core.Models;
using Berber.Core.Utils;
using Berber.Data;
using Berber.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.UI.MenuSystem
{
    public class CustomerBookingMenu
    {
        private readonly Customer _customer;
        private readonly ISalonManager _salonManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IAppointmentManager _appointmentManager;

        public CustomerBookingMenu(Customer customer)
        {
            _customer = customer;
            _salonManager = new SalonManager(Database.Salons);
            _employeeManager = new EmployeeManager();
            _appointmentManager = new AppointmentManager(Database.Appointments);
        }

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Book Appointment");

                ConsoleUIHelper.PrintOption(1, "Start Booking");
                ConsoleUIHelper.PrintOption(2, "My Appointments");
                ConsoleUIHelper.PrintOption(3, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1:
                        StartBooking();
                        break;

                    case 2:
                        ViewMyAppointments();
                        break;

                    case 3:
                        return;

                    default:
                        ConsoleUIHelper.PrintError("Invalid option.");
                        break;
                }
            }
        }
        private List<DateTime> GetNext7Days()
        {
            List<DateTime> days = new List<DateTime>();
            DateTime today = DateTime.Today;

            for (int i = 0; i < 7; i++)
            {
                days.Add(today.AddDays(i));
            }

            return days;
        }

        private DateTime ChooseDayFromNext7Days()
        {
            List<DateTime> days = GetNext7Days();

            ConsoleUIHelper.Title("Select a Day");

            for (int i = 0; i < days.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {days[i]:ddd yyyy-MM-dd}");
            }

            int choice = InputHelper.ReadInt("Choose day: ") - 1;

            if (choice < 0 || choice >= days.Count)
                return DateTime.MinValue;

            return days[choice];
        }

        private void StartBooking()
        {
            // 1. Choose salon
            Salon salon = ChooseSalon();
            if (salon == null) return;

            // 2. Choose service
            Service service = ChooseService(salon);
            if (service == null) return;

            // 3. Choose employee
            Employee employee = ChooseEmployee(salon, service);
            if (employee == null) return;

            // 4. Choose a day
            DateTime day = CalendarHelper.SelectDayFromNext7Days("Select a Day");
            if (day == DateTime.MinValue) return;

            // 5. Get available slots
            var slots = GenerateAvailableTimeSlots(salon, employee, service, day);

            if (slots.Count == 0)
            {
                ConsoleUIHelper.PrintError("No available slots for this day.");
                ConsoleUIHelper.Pause();
                return;
            }

            ConsoleUIHelper.Title("Available Slots");
            for (int i = 0; i < slots.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {slots[i]:HH:mm} - {slots[i].AddMinutes(service.DurationMinutes):HH:mm}");
            }

            int choice = InputHelper.ReadInt("Choose slot: ") - 1;
            if (choice < 0 || choice >= slots.Count) return;

            DateTime start = slots[choice];

            Appointment appt = new Appointment(
                Database.Appointments.Count + 1,
                _customer, salon, employee, service, start
            );

            Database.Appointments.Add(appt);

            ConsoleUIHelper.PrintSuccess("Appointment created (Pending).");
            ConsoleUIHelper.Pause();
        }
        private Salon ChooseSalon()
        {
            ConsoleUIHelper.Title("Choose Salon");

            foreach (Salon s in Database.Salons)
            {
                Console.WriteLine($"{s.Id} - {s.Name}");
            }

            int id = InputHelper.ReadInt("Enter Salon ID: ");
            return _salonManager.GetSalonById(id);
        }

        private Service ChooseService(Salon salon)
        {
            ConsoleUIHelper.Title("Choose Service");

            foreach (Service s in salon.Services)
            {
                Console.WriteLine($"{s.Id} - {s.Name} ({s.DurationMinutes} min)");
            }

            int id = InputHelper.ReadInt("Service ID: ");
            return salon.Services.Find(s => s.Id == id);
        }

        private Employee ChooseEmployee(Salon salon, Service service)
        {
            ConsoleUIHelper.Title("Choose Employee");

            List<Employee> available = _employeeManager.GetEmployeesWhoCanPerform(service, salon);

            if (available.Count == 0)
            {
                ConsoleUIHelper.PrintError("No employees can perform this service.");
                ConsoleUIHelper.Pause();
                return null;
            }

            foreach (Employee e in available)
            {
                Console.WriteLine($"{e.Id} - {e.Name}");
            }

            int id = InputHelper.ReadInt("Select Employee ID: ");
            return available.Find(e => e.Id == id);
        }

        private void ViewMyAppointments()
        {
            ConsoleUIHelper.Title("My Appointments");

            List<Appointment> myAppointments =
                _appointmentManager.GetAppointmentsForCustomer(_customer);

            if (myAppointments.Count == 0)
            {
                ConsoleUIHelper.PrintError("You have no appointments.");
            }
            else
            {
                foreach (Appointment a in myAppointments)
                {
                    Console.WriteLine(
                        $"#{a.Id} | {a.Service.Name} | {a.StartTime} | {a.Status}"
                    );
                }
            }

            ConsoleUIHelper.Pause();
        }
        private List<DateTime> GenerateAvailableTimeSlots(
            Salon salon,
            Employee employee,
            Service service,
            DateTime date)
        {
            List<DateTime> slots = new List<DateTime>();

            if (!salon.WorkingHours.ContainsKey(date.DayOfWeek))
                return slots;

            TimeRange range = salon.WorkingHours[date.DayOfWeek];

            DateTime dayStart = date.Date + range.Start;
            DateTime dayEnd = date.Date + range.End;

            DateTime lastStart = dayEnd.AddMinutes(-service.DurationMinutes);
            DateTime current = dayStart;

            while (current <= lastStart)
            {
                DateTime slotEnd = current.AddMinutes(service.DurationMinutes);

                bool employeeAvailable =
                    _employeeManager.IsEmployeeAvailable(employee, current, slotEnd);

                bool noConflict =
                    !_appointmentManager.HasConflict(employee, current, slotEnd);

                if (employeeAvailable && noConflict)
                    slots.Add(current);

                current = current.AddMinutes(15);
            }

            return slots;
        }


    }

}

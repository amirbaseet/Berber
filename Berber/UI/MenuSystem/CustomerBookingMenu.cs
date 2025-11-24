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
            // Step 1 — Choose Salon
            Salon salon = ChooseSalon();
            if (salon == null) return;

            // Step 2 — Choose Service
            Service service = ChooseService(salon);
            if (service == null) return;

            // Step 3 — Choose Employee
            Employee employee = ChooseEmployee(salon, service);
            if (employee == null) return;

            // Step 4 — Pick day from next 7 days
            DateTime date = ChooseDayFromNext7Days();
            if (date == DateTime.MinValue)
            {
                ConsoleUIHelper.PrintError("Invalid day selection.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Step 5 — Generate available slots for selected day
            List<DateTime> slots = GenerateAvailableTimeSlots(salon, employee, service, date);

            if (slots.Count == 0)
            {
                ConsoleUIHelper.PrintError("No available time slots for this day.");
                ConsoleUIHelper.Pause();
                return;
            }

            ConsoleUIHelper.Title(
                $"Available Slots for {date:ddd yyyy-MM-dd} ({service.DurationMinutes} min)"
            );

            for (int i = 0; i < slots.Count; i++)
            {
                DateTime start = slots[i];
                DateTime end = start.AddMinutes(service.DurationMinutes);

                Console.WriteLine($"{i + 1}. {start:HH:mm} - {end:HH:mm}");
            }

            int slotChoice = InputHelper.ReadInt("Choose time slot: ") - 1;

            if (slotChoice < 0 || slotChoice >= slots.Count)
            {
                ConsoleUIHelper.PrintError("Invalid time slot.");
                ConsoleUIHelper.Pause();
                return;
            }

            DateTime startTime = slots[slotChoice];

            // Step 6 — Create appointment
            int newId = Database.Appointments.Count + 1;

            Appointment appt = _appointmentManager.CreateAppointment(
                newId, _customer, salon, employee, service, startTime
            );

            if (appt == null)
            {
                ConsoleUIHelper.PrintError("Failed to create appointment.");
            }
            else
            {
                ConsoleUIHelper.PrintSuccess(
                    "Appointment created successfully!\nStatus: Pending (awaiting admin approval)"
                );
            }

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
        private List<DateTime> GenerateAvailableTimeSlots(Salon salon, Employee employee, Service service, DateTime date)
        {
            List<DateTime> slots = new List<DateTime>();

            // 1. Check if salon has working hours for that weekday
            if (!salon.WorkingHours.ContainsKey(date.DayOfWeek))
                return slots;

            TimeRange salonHours = salon.WorkingHours[date.DayOfWeek];

            // Convert time-only to full DateTime using selected day
            DateTime dayStart = new DateTime(date.Year, date.Month, date.Day,
                salonHours.Start.Hour, salonHours.Start.Minute, 0);

            DateTime dayEnd = new DateTime(date.Year, date.Month, date.Day,
                salonHours.End.Hour, salonHours.End.Minute, 0);

            // Cannot start after this time (service must fit in closing hours)
            DateTime lastPossibleStart = dayEnd.AddMinutes(-service.DurationMinutes);

            DateTime current = dayStart;

            while (current <= lastPossibleStart)
            {
                DateTime slotStart = current;
                DateTime slotEnd = slotStart.AddMinutes(service.DurationMinutes);

                bool employeeAvailable =
                    _employeeManager.IsEmployeeAvailable(employee, slotStart, slotEnd);

                bool noConflict =
                    !_appointmentManager.HasConflict(employee, slotStart, slotEnd);

                if (employeeAvailable && noConflict)
                    slots.Add(slotStart);

                current = current.AddMinutes(15);
            }

            return slots;
        }


    }

}

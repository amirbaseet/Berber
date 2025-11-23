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

            // Step 4 — Choose Date & Time
            DateTime startTime = InputHelper.ReadDateTime("Enter appointment start time (yyyy-MM-dd HH:mm): ");

            // Validate salon open
            if (!_salonManager.IsSalonOpen(salon, startTime))
            {
                ConsoleUIHelper.PrintError("Salon is not open at this time.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Step 5 — Check employee availability
            DateTime endTime = startTime.AddMinutes(service.DurationMinutes);

            if (!_employeeManager.IsEmployeeAvailable(employee, startTime, endTime))
            {
                ConsoleUIHelper.PrintError("Employee is not available during this time.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Step 6 — Check appointment conflicts
            if (_appointmentManager.HasConflict(employee, startTime, endTime))
            {
                ConsoleUIHelper.PrintError("Employee already has an appointment at this time.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Step 7 — Create the appointment
            int newId = Database.Appointments.Count + 1;

            Appointment appt = _appointmentManager.CreateAppointment(
                newId, _customer, salon, employee, service, startTime
            );

            if (appt == null)
            {
                ConsoleUIHelper.PrintError("Could not create appointment.");
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
    }

}

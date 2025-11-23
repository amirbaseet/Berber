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
    public class CustomerRescheduleMenu
    {
        private readonly Customer _customer;
        private readonly IAppointmentManager _appointmentManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly ISalonManager _salonManager;

        public CustomerRescheduleMenu(Customer customer)
        {
            _customer = customer;
            _appointmentManager = new AppointmentManager(Database.Appointments);
            _employeeManager = new EmployeeManager();
            _salonManager = new SalonManager(Database.Salons);
        }

        public void Show()
        {
            ConsoleUIHelper.Title("Reschedule Appointment");

            List<Appointment> myAppointments =
                _appointmentManager.GetAppointmentsForCustomer(_customer);

            if (myAppointments.Count == 0)
            {
                ConsoleUIHelper.PrintError("You have no appointments to reschedule.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Display all appointments
            foreach (Appointment a in myAppointments)
            {
                Console.WriteLine(
                    $"#{a.Id} | {a.Service.Name} | {a.StartTime:yyyy-MM-dd HH:mm} | Status: {a.Status}"
                );
            }

            int id = InputHelper.ReadInt("Enter appointment ID to reschedule: ");
            Appointment appointment = Database.Appointments.Find(a => a.Id == id);

            if (appointment == null || appointment.Customer != _customer)
            {
                ConsoleUIHelper.PrintError("Invalid appointment ID.");
                ConsoleUIHelper.Pause();
                return;
            }

            ConsoleUIHelper.Line();
            Console.WriteLine("Enter new date and time:");

            DateTime newStart = InputHelper.ReadDateTime("New start (yyyy-MM-dd HH:mm): ");
            DateTime newEnd = newStart.AddMinutes(appointment.Service.DurationMinutes);

            // Validate salon open
            if (!_salonManager.IsSalonOpen(appointment.Salon, newStart))
            {
                ConsoleUIHelper.PrintError("Salon is not open at that time.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Check employee availability
            if (!_employeeManager.IsEmployeeAvailable(appointment.Employee, newStart, newEnd))
            {
                ConsoleUIHelper.PrintError("Employee is not available during this time.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Check for conflicts with other appointments
            if (_appointmentManager.HasConflict(appointment.Employee, newStart, newEnd))
            {
                ConsoleUIHelper.PrintError("Employee has another appointment at this time.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Update appointment times
            appointment.StartTime = newStart;
            appointment.EndTime = newEnd;
            appointment.Status = Core.Models.Enums.AppointmentStatus.Pending; // reset approval

            ConsoleUIHelper.PrintSuccess(
                "Appointment successfully rescheduled!\nStatus reset to Pending."
            );

            ConsoleUIHelper.Pause();
        }
    }
}

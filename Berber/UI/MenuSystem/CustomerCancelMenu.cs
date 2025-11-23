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
    public class CustomerCancelMenu
    {
        private readonly Customer _customer;
        private readonly IAppointmentManager _appointmentManager;

        public CustomerCancelMenu(Customer customer)
        {
            _customer = customer;
            _appointmentManager = new AppointmentManager(Database.Appointments);
        }

        public void Show()
        {
            ConsoleUIHelper.Title("Cancel Appointment");

            List<Appointment> myAppointments =
                _appointmentManager.GetAppointmentsForCustomer(_customer);

            if (myAppointments.Count == 0)
            {
                ConsoleUIHelper.PrintError("You have no appointments to cancel.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Display appointments
            foreach (Appointment a in myAppointments)
            {
                Console.WriteLine(
                    $"#{a.Id} | {a.Service.Name} | {a.StartTime:yyyy-MM-dd HH:mm} | {a.Status}");
            }

            int id = InputHelper.ReadInt("Enter Appointment ID to cancel: ");
            Appointment appointment = Database.Appointments.Find(a => a.Id == id);

            if (appointment == null || appointment.Customer != _customer)
            {
                ConsoleUIHelper.PrintError("Invalid appointment ID.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Remove from employee schedule
            appointment.Employee.Appointments.Remove(appointment);

            // Mark as cancelled
            _appointmentManager.CancelAppointment(appointment);

            ConsoleUIHelper.PrintSuccess("Appointment cancelled successfully.");
            ConsoleUIHelper.Pause();
        }
    }

}

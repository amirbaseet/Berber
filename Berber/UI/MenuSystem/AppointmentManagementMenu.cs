using Berber.Core.Interfaces;
using Berber.Core.Managers;
using Berber.Core.Models.Enums;
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
    public class AppointmentManagementMenu
    {
        private readonly IAppointmentManager _appointmentManager;

        public AppointmentManagementMenu()
        {
            _appointmentManager = new AppointmentManager(Database.Appointments);
        }

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Appointment Management");

                ConsoleUIHelper.PrintOption(1, "View All Appointments");
                ConsoleUIHelper.PrintOption(2, "View Pending Appointments");
                ConsoleUIHelper.PrintOption(3, "Approve Appointment");
                ConsoleUIHelper.PrintOption(4, "Reject Appointment");
                ConsoleUIHelper.PrintOption(5, "Cancel Appointment");
                ConsoleUIHelper.PrintOption(6, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1: ViewAll(); break;
                    case 2: ViewPending(); break;
                    case 3: Approve(); break;
                    case 4: Reject(); break;
                    case 5: Cancel(); break;
                    case 6: return;

                    default:
                        ConsoleUIHelper.PrintError("Invalid option.");
                        break;
                }
            }
        }

        private void ViewAll()
        {
            ConsoleUIHelper.Title("All Appointments");

            if (Database.Appointments.Count == 0)
            {
                ConsoleUIHelper.PrintError("No appointments available.");
            }
            else
            {
                foreach (Appointment a in Database.Appointments)
                {
                    Console.WriteLine(
                        $"#{a.Id} | {a.Customer.Name} | {a.Service.Name} | {a.StartTime} | {a.Status}");
                }
            }

            ConsoleUIHelper.Pause();
        }

        private void ViewPending()
        {
            ConsoleUIHelper.Title("Pending Appointments");

            bool found = false;

            foreach (Appointment a in Database.Appointments)
            {
                if (a.Status == AppointmentStatus.Pending)
                {
                    Console.WriteLine(
                        $"#{a.Id} | {a.Customer.Name} | {a.Service.Name} | {a.StartTime}");
                    found = true;
                }
            }

            if (!found)
                ConsoleUIHelper.PrintError("No pending appointments.");

            ConsoleUIHelper.Pause();
        }

        private void Approve()
        {
            ConsoleUIHelper.Title("Approve Appointment");

            int id = InputHelper.ReadInt("Enter Appointment ID: ");
            Appointment appt = Database.Appointments.Find(a => a.Id == id);

            if (appt == null)
            {
                ConsoleUIHelper.PrintError("Appointment not found.");
            }
            else
            {
                _appointmentManager.ApproveAppointment(appt);
                ConsoleUIHelper.PrintSuccess("Appointment approved.");
            }

            ConsoleUIHelper.Pause();
        }

        private void Reject()
        {
            ConsoleUIHelper.Title("Reject Appointment");

            int id = InputHelper.ReadInt("Enter Appointment ID: ");
            Appointment appt = Database.Appointments.Find(a => a.Id == id);

            if (appt == null)
            {
                ConsoleUIHelper.PrintError("Appointment not found.");
            }
            else
            {
                _appointmentManager.RejectAppointment(appt);
                ConsoleUIHelper.PrintSuccess("Appointment rejected.");
            }

            ConsoleUIHelper.Pause();
        }

        private void Cancel()
        {
            ConsoleUIHelper.Title("Cancel Appointment");

            int id = InputHelper.ReadInt("Enter Appointment ID: ");
            Appointment appt = Database.Appointments.Find(a => a.Id == id);

            if (appt == null)
            {
                ConsoleUIHelper.PrintError("Appointment not found.");
            }
            else
            {
                _appointmentManager.CancelAppointment(appt);
                ConsoleUIHelper.PrintSuccess("Appointment cancelled.");
            }

            ConsoleUIHelper.Pause();
        }
    }

}

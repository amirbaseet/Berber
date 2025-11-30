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

            // Get customer appointments
            List<Appointment> myAppointments =
                _appointmentManager.GetAppointmentsForCustomer(_customer);

            if (myAppointments.Count == 0)
            {
                ConsoleUIHelper.PrintError("You have no appointments to reschedule.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Show appointments
            foreach (Appointment a in myAppointments)
            {
                Console.WriteLine(
                    $"#{a.Id} | {a.Service.Name} | {a.StartTime:yyyy-MM-dd HH:mm} | Status: {a.Status}"
                );
            }

            int id = InputHelper.ReadInt("Enter appointment ID to reschedule: ");
            Appointment appointment = Database.Appointments.FirstOrDefault(a => a.Id == id);

            if (appointment == null || appointment.Customer != _customer)
            {
                ConsoleUIHelper.PrintError("Invalid appointment ID.");
                ConsoleUIHelper.Pause();
                return;
            }

            ConsoleUIHelper.Line();

            // -----------------------------
            // 1. SELECT NEW DAY (NEXT 7 DAYS)
            // -----------------------------
            DateTime newDay = CalendarHelper.SelectDayFromNext7Days("Select a new day");

            if (newDay == DateTime.MinValue)
            {
                ConsoleUIHelper.PrintError("Invalid selection.");
                ConsoleUIHelper.Pause();
                return;
            }

            // -----------------------------
            // 2. GET AVAILABLE SLOTS
            // -----------------------------
            List<DateTime> availableSlots =
                GenerateAvailableTimeSlots(
                    appointment.Salon,
                    appointment.Employee,
                    appointment.Service,
                    newDay
                );

            if (availableSlots.Count == 0)
            {
                ConsoleUIHelper.PrintError("No available slots for the selected day.");
                ConsoleUIHelper.Pause();
                return;
            }

            ConsoleUIHelper.Title("Available Time Slots");

            for (int i = 0; i < availableSlots.Count; i++)
            {
                DateTime s = availableSlots[i];
                Console.WriteLine($"{i + 1}. {s:HH:mm} - {s.AddMinutes(appointment.Service.DurationMinutes):HH:mm}");
            }

            // -----------------------------
            // 3. SELECT SLOT
            // -----------------------------
            int slotChoice = InputHelper.ReadInt("Choose a slot: ") - 1;

            if (slotChoice < 0 || slotChoice >= availableSlots.Count)
            {
                ConsoleUIHelper.PrintError("Invalid slot.");
                ConsoleUIHelper.Pause();
                return;
            }

            DateTime newStart = availableSlots[slotChoice];
            DateTime newEnd = newStart.AddMinutes(appointment.Service.DurationMinutes);

            // -----------------------------
            // 4. VALIDATIONS
            // -----------------------------
            if (!_salonManager.IsSalonOpen(appointment.Salon, newStart))
            {
                ConsoleUIHelper.PrintError("Salon is not open at this time.");
                ConsoleUIHelper.Pause();
                return;
            }

            if (!_employeeManager.IsEmployeeAvailable(appointment.Employee, newStart, newEnd))
            {
                ConsoleUIHelper.PrintError("Employee is not available at this time.");
                ConsoleUIHelper.Pause();
                return;
            }

            if (_appointmentManager.HasConflict(appointment.Employee, newStart, newEnd))
            {
                ConsoleUIHelper.PrintError("Employee already has an appointment at this time.");
                ConsoleUIHelper.Pause();
                return;
            }

            // -----------------------------
            // 5. APPLY RESCHEDULE
            // -----------------------------
            appointment.StartTime = newStart;
            appointment.EndTime = newEnd;
            appointment.Status = Core.Models.Enums.AppointmentStatus.Pending; // reset approval

            ConsoleUIHelper.PrintSuccess(
                "Appointment successfully rescheduled!\nStatus reset to Pending."
            );

            ConsoleUIHelper.Pause();
        }

        // =====================================================================
        // Generate available time slots (SAME LOGIC AS BOOKING)
        // =====================================================================

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

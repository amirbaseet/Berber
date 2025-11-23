using Berber.Core.Interfaces;
using Berber.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Managers
{
    public class AppointmentManager : IAppointmentManager
    {
        private readonly List<Appointment> _appointments;

        public AppointmentManager(List<Appointment> appointments)
        {
            _appointments = appointments;
        }

        public Appointment CreateAppointment(int id, Customer customer, Salon salon,
                                             Employee employee, Service service,
                                             DateTime startTime)
        {
            DateTime endTime = startTime.AddMinutes(service.DurationMinutes);

            if (HasConflict(employee, startTime, endTime))
                return null;

            Appointment appt = new Appointment(id, customer, salon, employee, service, startTime);

            _appointments.Add(appt);
            employee.Appointments.Add(appt);

            return appt;
        }

        public bool HasConflict(Employee employee, DateTime start, DateTime end)
        {
            foreach (Appointment a in employee.Appointments)
            {
                if (start < a.EndTime && end > a.StartTime)
                    return true;
            }
            return false;
        }

        public List<Appointment> GetAppointmentsForEmployee(Employee employee)
        {
            return _appointments.Where(a => a.Employee == employee).ToList();
        }

        public List<Appointment> GetAppointmentsForCustomer(Customer customer)
        {
            return _appointments.Where(a => a.Customer == customer).ToList();
        }

        public void ApproveAppointment(Appointment appointment)
        {
            appointment.Approve();
        }

        public void RejectAppointment(Appointment appointment)
        {
            appointment.Reject();
        }

        public void CancelAppointment(Appointment appointment)
        {
            appointment.Cancel();
        }
    }
}

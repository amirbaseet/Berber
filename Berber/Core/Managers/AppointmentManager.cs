using Berber.Core.Interfaces;
using Berber.Core.Models;
using Berber.Core.Models.Enums;
using Berber.Data;
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
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (salon == null) throw new ArgumentNullException(nameof(salon));
            if (employee == null) throw new ArgumentNullException(nameof(employee));
            if (service == null) throw new ArgumentNullException(nameof(service));

            DateTime endTime = startTime.AddMinutes(service.DurationMinutes);
            if (!salon.Employees.Contains(employee))
                throw new InvalidOperationException(
                    $"Employee {employee.Name} does not work at salon {salon.Name}."
                );
            var appointment = new Appointment(id, customer, salon, employee, service, startTime);

            Database.Appointments.Add(appointment);
            employee.Appointments.Add(appointment);

            return appointment;
        }

        public bool HasConflict(Employee emp, DateTime start, DateTime end)
        {
            return _appointments.Any(a =>
                a.Employee.Id == emp.Id &&
                a.Status != AppointmentStatus.Cancelled &&
                a.StartTime < end &&
                a.EndTime > start
            );
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

using Berber.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Interfaces
{
    public interface IAppointmentManager
    {
        Appointment CreateAppointment(int id, Customer customer, Salon salon, Employee employee,
                                     Service service, DateTime startTime);

        bool HasConflict(Employee employee, DateTime start, DateTime end);
        List<Appointment> GetAppointmentsForEmployee(Employee employee);
        List<Appointment> GetAppointmentsForCustomer(Customer customer);

        void ApproveAppointment(Appointment appointment);
        void RejectAppointment(Appointment appointment);
        void CancelAppointment(Appointment appointment);
    }
}

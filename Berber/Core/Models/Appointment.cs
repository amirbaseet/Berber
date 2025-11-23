using Berber.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public Salon Salon { get; set; }
        public Employee Employee { get; set; }
        public Service Service { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public AppointmentStatus Status { get; set; }

        public Appointment(int id, Customer customer, Salon salon, Employee employee,
                           Service service, DateTime startTime)
        {
            Id = id;
            Customer = customer;
            Salon = salon;
            Employee = employee;
            Service = service;

            StartTime = startTime;
            EndTime = startTime.AddMinutes(service.DurationMinutes);

            Status = AppointmentStatus.Pending;
        }

        /// <summary>
        /// Checks if this appointment overlaps another.
        /// </summary>
        public bool ConflictsWith(Appointment other)
        {
            return StartTime < other.EndTime && EndTime > other.StartTime;
        }

        public void Approve()
        {
            Status = AppointmentStatus.Approved;
        }

        public void Reject()
        {
            Status = AppointmentStatus.Rejected;
        }

        public void Cancel()
        {
            Status = AppointmentStatus.Cancelled;
        }

        public override string ToString()
        {
            return $"#{Id} | {Service.Name} | {StartTime:HH:mm} - {EndTime:HH:mm} | {Status}";
        }
    }
}

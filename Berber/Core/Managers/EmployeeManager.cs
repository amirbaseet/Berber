using Berber.Core.Interfaces;
using Berber.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Managers
{
    public class EmployeeManager : IEmployeeManager
    {
        public void AddAvailability(Employee employee, TimeRange availability)
        {
            employee.Availability.Add(availability);
        }

        public bool IsEmployeeAvailable(Employee e, DateTime start, DateTime end)
        {
            foreach (TimeRange range in e.Availability)
            {
                // Convert availability (time-of-day) to full DateTime of the booking day
                DateTime availStart = new DateTime(
                    start.Year, start.Month, start.Day,
                    range.Start.Hour, range.Start.Minute, 0
                );

                DateTime availEnd = new DateTime(
                    start.Year, start.Month, start.Day,
                    range.End.Hour, range.End.Minute, 0
                );

                if (start >= availStart && end <= availEnd)
                    return true;
            }

            return false;
        }

        public void AssignService(Employee employee, Service service)
        {
            if (!employee.ServicesCanDo.Contains(service))
                employee.ServicesCanDo.Add(service);
        }

        public List<Employee> GetEmployeesWhoCanPerform(Service service, Salon salon)
        {
            return salon.Employees
                        .Where(e => e.ServicesCanDo.Contains(service))
                        .ToList();
        }
    }
}

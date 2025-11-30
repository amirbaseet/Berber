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

        public bool IsEmployeeAvailable(Employee emp, DateTime start, DateTime end)
        {
            TimeSpan startT = start.TimeOfDay;
            TimeSpan endT = end.TimeOfDay;

            foreach (var av in emp.Availability)
            {
                if (startT >= av.Start && endT <= av.End)
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

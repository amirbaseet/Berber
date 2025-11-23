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

        public bool IsEmployeeAvailable(Employee employee, DateTime start, DateTime end)
        {
            foreach (TimeRange slot in employee.Availability)
            {
                if (start >= slot.Start && end <= slot.End)
                {
                    return true;
                }
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

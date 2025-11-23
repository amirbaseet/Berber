using Berber.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Interfaces
{
    public interface IEmployeeManager
    {
        void AddAvailability(Employee employee, TimeRange availability);
        bool IsEmployeeAvailable(Employee employee, DateTime start, DateTime end);
        void AssignService(Employee employee, Service service);
        List<Employee> GetEmployeesWhoCanPerform(Service service, Salon salon);
    }
}

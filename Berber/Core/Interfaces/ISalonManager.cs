using Berber.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Interfaces
{
    public interface ISalonManager
    {
        void AddSalon(Salon salon);
        void AddServiceToSalon(Salon salon, Service service);
        void AddEmployeeToSalon(Salon salon, Employee employee);
        Salon GetSalonById(int id);
        List<Salon> GetAllSalons();
        bool IsSalonOpen(Salon salon, DateTime dateTime);
    }
}

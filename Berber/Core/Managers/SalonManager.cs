using Berber.Core.Interfaces;
using Berber.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Managers
{
    public class SalonManager : ISalonManager
    {
        private readonly List<Salon> _salons;

        public SalonManager(List<Salon> salons)
        {
            _salons = salons;
        }

        public void AddSalon(Salon salon)
        {
            _salons.Add(salon);
        }

        public void AddServiceToSalon(Salon salon, Service service)
        {
            salon.AddService(service);
        }

        public void AddEmployeeToSalon(Salon salon, Employee employee)
        {
            salon.AddEmployee(employee);
        }

        public Salon GetSalonById(int id)
        {
            return _salons.FirstOrDefault(s => s.Id == id);
        }

        public List<Salon> GetAllSalons()
        {
            return _salons;
        }

        public bool IsSalonOpen(Salon salon, DateTime dt)
        {
            if (!salon.WorkingHours.ContainsKey(dt.DayOfWeek))
                return false;

            TimeRange range = salon.WorkingHours[dt.DayOfWeek];

            // Convert time-only to full DateTime for the selected date
            DateTime openTime = new DateTime(
                dt.Year, dt.Month, dt.Day,
                range.Start.Hour, range.Start.Minute, 0
            );

            DateTime closeTime = new DateTime(
                dt.Year, dt.Month, dt.Day,
                range.End.Hour, range.End.Minute, 0
            );

            return dt >= openTime && dt <= closeTime;
        }
    }
}

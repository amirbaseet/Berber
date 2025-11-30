using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private int _durationMinutes;
        public int DurationMinutes
        {
            get => _durationMinutes;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Duration must be positive.");
                _durationMinutes = value;
            }
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        public Service(int id, string name, int durationMinutes, decimal price)
        {
            Id = id;
            Name = name;
            DurationMinutes = durationMinutes;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Name} ({DurationMinutes} min, {Price:C})";
        }
    }
}

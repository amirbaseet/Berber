using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Models
{
    public class TimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TimeRange(DateTime start, DateTime end)
        {
            if (end <= start)
                throw new ArgumentException("End time must be after start time.");

            Start = start;
            End = end;
        }

        /// <summary>
        /// Checks if a specific moment is inside this time range.
        /// </summary>
        public bool Contains(DateTime time)
        {
            return time >= Start && time <= End;
        }

        /// <summary>
        /// Checks whether this time range overlaps another time range.
        /// </summary>
        public bool Overlaps(TimeRange other)
        {
            return Start < other.End && End > other.Start;
        }

        public override string ToString()
        {
            return $"{Start:yyyy-MM-dd HH:mm} - {End:yyyy-MM-dd HH:mm}";
        }

    }
}

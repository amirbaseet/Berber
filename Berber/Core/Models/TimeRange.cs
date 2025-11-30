using System;

namespace Berber.Core.Models
{
    public class TimeRange
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public TimeRange(TimeSpan start, TimeSpan end)
        {
            if (end <= start)
                throw new ArgumentException("End time must be after start time.");

            Start = start;
            End = end;
        }

        /// <summary>
        /// Checks if a specific DateTime falls inside the time range (based on TimeOfDay)
        /// </summary>
        public bool Contains(DateTime dateTime)
        {
            TimeSpan time = dateTime.TimeOfDay;
            return time >= Start && time <= End;
        }

        /// <summary>
        /// Checks if two time-only ranges overlap.
        /// </summary>
        public bool Overlaps(TimeRange other)
        {
            return Start < other.End && End > other.Start;
        }

        public override string ToString()
        {
            return $"{Start:hh\\:mm} - {End:hh\\:mm}";
        }
    }
}

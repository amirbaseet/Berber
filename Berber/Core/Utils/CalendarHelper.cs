using Berber.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.Core.Utils
{
    public class CalendarHelper
    {
        public static List<DateTime> GetNext7Days()
        {
            List<DateTime> days = new List<DateTime>();
            DateTime today = DateTime.Today;

            for (int i = 0; i < 7; i++)
                days.Add(today.AddDays(i));

            return days;
        }

        public static DateTime SelectDayFromNext7Days(string title)
        {
            ConsoleUIHelper.Title(title);

            List<DateTime> days = GetNext7Days();

            for (int i = 0; i < days.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {days[i]:ddd yyyy-MM-dd}");
            }

            int choice = InputHelper.ReadInt("Choose: ") - 1;

            if (choice < 0 || choice >= days.Count)
                return DateTime.MinValue;

            return days[choice];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.UI.Helpers
{
    public static class InputHelper
    {
        public static int ReadInt(string message)
        {
            int value;
            Console.Write(message);

            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid number. Try again: ");
            }

            return value;
        }

        public static decimal ReadDecimal(string message)
        {
            decimal value;
            Console.Write(message);

            while (!decimal.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid value. Try again: ");
            }

            return value;
        }

        public static string ReadString(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("Input cannot be empty. Try again: ");
                input = Console.ReadLine();
            }

            return input;
        }

        public static DateTime ReadDateTime(string message)
        {
            DateTime value;
            Console.Write(message);

            while (!DateTime.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid date/time format. Try again: ");
            }

            return value;
        }
    }

}

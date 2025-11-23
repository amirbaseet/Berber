using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.UI.Helpers
{
    public static class ConsoleUIHelper
    {
        public static void Title(string text)
        {
            Console.Clear();
            Console.WriteLine("=======================================");
            Console.WriteLine(text.ToUpper());
            Console.WriteLine("=======================================");
        }

        public static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
        }

        public static void PrintOption(int number, string text)
        {
            Console.WriteLine($"{number}. {text}");
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Line()
        {
            Console.WriteLine("---------------------------------------");
        }
    }

}

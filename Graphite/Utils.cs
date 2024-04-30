using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphite
{
    public static class Utils
    {
        public static void awaitEnter()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadKey();
        }
        public static void Line()
        {
            Console.WriteLine("\n===========================================================\n");
        }

        public static char GetChar(string prompt, out bool exitClause)
        {
            Console.Write(prompt);
            char choice = char.ToLower(Console.ReadKey().KeyChar);
            exitClause = (choice == 'q');
            return choice;
        }

        public static void displayList(List<string> aList)
        {
            Console.Write($"[ ");

            foreach (string item in aList)
            {
                Console.Write($"{item},");
            }
            Console.Write($"]");
        }


        public static



    }
}

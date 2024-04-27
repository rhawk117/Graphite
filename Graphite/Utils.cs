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
    }
}

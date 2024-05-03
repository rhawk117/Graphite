using System;
using System.Threading;

namespace Graphite
{
    public static class Prompt
    {
        private static void reset() => Console.ResetColor();
        public static void Question(string question)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"\n[ ? ] {question}: ");
            reset();
        }
        public static void Error(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n\t[ ! ] {error} [ ! ]\n");
            reset();
        }
        public static void Wait()
        {
            Console.Write("\n\t[ i ] Press ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ENTER ");
            reset();
            Console.Write("to continue... [ i ]\n");
            Console.ReadKey();
        }
        public static void Info(string info)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n\t[ i ] {info} [ i ]\n");
            reset();
        }
        public static void ColorMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            reset();
        }
        public static void ErrorHandler(Exception ex)
        {
            Utils.Line();
            Console.WriteLine($"Message\n{ex.Message}\n");
            Utils.Line();
            Wait();
            Console.WriteLine($"Traceback {ex.StackTrace}");
            Utils.Line();
            Wait();
            Console.WriteLine($"Exception Info \n {ex}");
            Wait();
        }
        public static string Menuify(string menuText)
        {
            string line = new string('=', 50);
            menuText = $"{line}\n[ ? ] {menuText} [ ? ]\n{line}";
            return menuText;
        }
        public static void DelayMessage(string message, int delay)
        {
            Info(message);
            Thread.Sleep(delay);
        }
    }
}

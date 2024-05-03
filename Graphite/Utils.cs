using System;
using System.Collections.Generic;

namespace Graphite
{
    public static class Utils
    {
        public static void Line()
        {
            Console.WriteLine("\n===========================================================\n");
        }

        public static string GetInput(string prompt, out bool exitClause)
        {
            Prompt.Question(prompt);
            string choice = Console.ReadLine();
            exitClause = (choice.ToLower() == "q");
            return choice;
        }

        public static void InlineGraph(List<string> aList)
        {
            foreach (string item in aList)
            {
                Console.Write($"{item},");
            }
        }

        public static string EdgeInfo(string from = "?", string to = "?")
        {
            return $"\n< {from} > ---------> < {to} >";
        }

        public static double GetIntput(string prompt)
        {
            Prompt.Question(prompt);
            string rawInput = Console.ReadLine();
            if (rawInput == "n")
            {
                return -1;
            }
            else if (!double.TryParse(rawInput, out double result) || result < 0)
            {
                Prompt.Error("Invalid Input, please provide a positive double");
                return GetIntput(prompt);
            }
            else
            {
                return result;
            }
        }

        public static void DisplayHeader()
        {
            Console.WriteLine(@"
            ===================================================================================
                ___                                     __          __                 __             
               /  /                                    /\ \      __/\ \__             /\ `\           
              /  /            __   _ __    __     _____\ \ \___ /\_\ \ ,_\    __      \ `\ `\         
            /<  <           /'_ `\/\`'__\/'__`\  /\ '__`\ \  _ `\/\ \ \ \/  /'__`\     `\ >  >        
            \ `\ `\        /\ \L\ \ \ \//\ \L\.\_\ \ \L\ \ \ \ \ \ \ \ \ \_/\  __/       /  /         
             `\ `\_|       \ \____ \ \_\\ \__/.\_\\ \ ,__/\ \_\ \_\ \_\ \__\ \____\     /\_/          
               `\//         \/___L\ \/_/ \/__/\/_/ \ \ \/  \/_/\/_/\/_/\/__/\/____/     \//           
                              /\____/               \ \_\                                             
                              \_/__/                 \/_/                                             
            *================================================================================*
            |                           Graphite - C# Console Graph UI                       |
            |                                made by: @rhawk117                              |
            |                             [ Press ENTER to continue ]                        |
            *================================================================================*");
            Console.ReadKey();

        }



    }
}

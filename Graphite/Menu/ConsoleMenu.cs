using System;
using System.Collections.Generic;
using static System.Console;

namespace Graphite.Menu
{
    public class ConsoleMenu
    {
        public List<string> Options { get; set; }
        private string Prompt { get; set; }

        private int highlight;

        private ConsoleMenu prevMenu;

        private int Highlight
        {
            get => highlight;
            set
            {
                if (value < 0)
                {
                    highlight = Options.Count - 1;
                }
                else if (value >= Options.Count)
                {
                    highlight = 0;
                }
                else
                {
                    highlight = value;
                }
            }
        }
        public ConsoleMenu(List<string> options, string prompt)
        {
            Options = options;
            Prompt = prompt;
            Highlight = 0;
        }

        public ConsoleMenu(List<string> options, string prompt, ConsoleMenu Previous)
        {
            Options = options;
            Prompt = prompt;
            Highlight = 0;
            Options.Add("Back");
            prevMenu = Previous;
        }
        public void GoBack()
        {
            if (prevMenu == null)
            {
                WriteLine("No previous menu set");
                return;
            }
            prevMenu.Run();
        }
        private void Show()
        {
            Clear();
            WriteLine(Prompt);
            for (int i = 0; i < Options.Count; i++)
            {

                string currentOption = Options[i], prefix = "";
                if (i == Highlight)
                {
                    prefix = ">> ";
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }
                WriteLine($"{prefix} [ " + currentOption + " ]");
            }
            ResetColor();
        }
        public string Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Clear();
                Show();
                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;
                switch (keyPressed)
                {
                    case ConsoleKey.UpArrow:
                        Highlight--;
                        break;

                    case ConsoleKey.DownArrow:
                        Highlight++;
                        break;
                }
            }
            while (keyPressed != ConsoleKey.Enter);
            return Options[highlight];
        }
    }
}
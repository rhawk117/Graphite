using System;
using System.Collections.Generic;
using static System.Console;

namespace Graphite
{
    // re-usable menu ui 
    public class ConsoleMenu
    {
        private List<string> menuItems; // menu options 

        private string message; // menu prompt 

        private int highlight;
        private int Highlight // current index selected or "highlighted" in menu
        {
            get => highlight;

            set
            {
                if (value < 0)
                    highlight = menuItems.Count - 1;

                else if (value >= menuItems.Count)
                    highlight = 0;

                else
                    highlight = value;
            }
        }
        private const string BACK = "Go Back";

        public ConsoleMenu(List<string> options, string prompt)
        {
            menuItems = options;
            message = Prompt.Menuify(prompt);
            Highlight = 0;
        }

        public void AddBack() => menuItems.Add(BACK);

        private void showOptions()
        {
            Clear();
            WriteLine(message);
            for (int i = 0; i < menuItems.Count; i++)
            {
                string currentOption = menuItems[i], prefix = "";
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
                WriteLine($"{prefix} [ {currentOption} ]");
            }
            ResetColor();
        }

        public string Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Clear();
                showOptions();
                keyPressed = ReadKey(true).Key;
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

            return menuItems[highlight];
        }
    }
}
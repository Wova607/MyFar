using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFar_v1
{
    class MainMenu
    {
        private string[] menu;
        private string[] bg;
        public MainMenu()
        {
            bg = new string[139];
            for (int i = 0; i < 139; i++)
            {
                bg[i] = " ";
            }
            menu = new string[8];
            menu[0] = "1 ChenDrv";
            menu[1] = "2 View  "; 
            menu[2] = "3 Move   ";
            menu[3] = "4 Delete   "; 
            menu[4] = "5 Copy ";
            menu[5] = "6 Rename   ";
            menu[6] = "7 MkDir   ";
            menu[7] = "8 EXIT   ";

            
        }
        public void ShowMenu()
        {
            Console.SetCursorPosition(0, 36);
            for (int i = 0; i <139; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write(bg[i]);
            }
            Console.ResetColor();

            int x = 2; int y = 36;
            for (int i = 0; i < menu.Length; i++)
            {
                Console.SetCursorPosition(x, y);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(menu[i]);
                    Console.ResetColor();
                
                x +=18; 
            }


        }
    }
}

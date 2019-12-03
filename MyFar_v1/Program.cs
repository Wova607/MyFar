using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFar_v1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 37;  //фіксуємо розміри вікна
            Console.WindowWidth = 139;
            Console.CursorVisible = false;
            

            Manager manager = new Manager();
            
        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Threading;

namespace MyFar_v1
{
    class Panel
    {
        char[,] panel;
        int width = 69;
        int hight = 36;
        public int PanelX { get; set; }
        public int PanelY { get; set; }
        public Tree MyTree;
        char[] select;


        public Panel()
        {
            panel = new char[hight, width];
            for (int i = 0; i < hight; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    panel[i, j] = '\0';
                }
            }

            for (int i = 0; i < hight; i++)
            {
                for (int j = 0; j < width; j++)
                {

                    panel[0, j] = '=';
                    panel[hight - 1, j] = '=';
                    panel[33, j] = '=';

                }

            }

            for (int i = 1; i < hight - 1; i++)
            {
                for (int j = 0; j < width; j++)
                {

                    panel[i, 0] = '|';
                    panel[i, 1] = '|';
                    panel[i, width - 1] = '|';
                    panel[i, width - 2] = '|';
                    if (i < 33)
                    {
                        panel[i, 35] = '|';
                        panel[i, 50] = '|';
                    }


                }

            }

            MyTree = new Tree();
            select = new char[65];
            for (int i = 0; i < 65; i++)
                select[i] = '\0';

        }
        public void ShowSelect(int x, int y)
        {

            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i < 65; i++)
                Console.Write(select[i]);

        }
        public void CorectMoveUP(int x, int y)
        {

            Console.SetCursorPosition(x, ++y);   //корегуємо нижній запис
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            select[33] = '|';
            select[48] = '|';
            for (int i = 0; i < 65; i++)
                Console.Write(select[i]);
            Console.ResetColor();
        }
        public void CorectMoveDown(int x, int y)
        {
            Console.SetCursorPosition(x, --y); //корегуємо верхній запис
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            select[33] = '|';
            select[48] = '|';
            for (int i = 0; i < 65; i++)
                Console.Write(select[i]);
            Console.ResetColor();
        }
        public void ShowPanel()
        {

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            int x = PanelX; int y = PanelY;
            for (int i = 0; i < hight; i++)
            {
                Console.SetCursorPosition(x, y);
                for (int j = 0; j < width; j++)
                {
                    Console.Write(panel[i, j]);
                }
                y++;
                Console.WriteLine();
            }
            y = PanelY;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(x + 17, y + 1);
            Console.WriteLine("Name");
            Console.SetCursorPosition(x + 41, y + 1);
            Console.WriteLine("Type");
            Console.SetCursorPosition(x + 57, y + 1);
            Console.WriteLine("Date");
            Console.ResetColor();

            //виведемо шлях

            ShowPath();

            // вільне місце на драйві
            x = PanelX; y = PanelY;
            Console.BackgroundColor = ConsoleColor.DarkCyan; ;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(x + 3, y + 34);
            MyTree.FreeSpace = MyTree.ActualDriver.TotalFreeSpace;
            Console.WriteLine($" Drive \"{MyTree.ActualDriver.Name}\" Free {MyTree.FreeSpace / 1024 / 1024} Mb ");
            Console.ResetColor();

        }
        public void ShowPath()
        {
            int x = PanelX; int y = PanelY;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(x + 7, y);
            MyTree.Path = MyTree.ActualeDirectory.FullName;
            while (MyTree.Path.Length > 52)
            {
                MyTree.Path = CutPath(MyTree.Path);
            }

            Console.WriteLine($"Path: {MyTree.Path}");
            Console.ResetColor();
        }
        public String CutPath(string path)
        {
            string replaseString = null;
            int i = 0;
            if (MyTree.Path.IndexOf("...") == 3)
            {
                i = 8;
                replaseString = "...\\";
            }
            else
                i = 3;

            while (MyTree.Path[i] != '\\' || (MyTree.Path.Length - replaseString.Length) > 50)
            {
                replaseString += MyTree.Path[i];
                i++;
            }

            return path.Replace(replaseString, "...");
        }
        public void ChangeDriver()
        {
            ShowDrvPanel();

            int k = 0;
            ShowDrvList(ref k);
            bool exit = false;

            while (!exit)
            {
                switch (Console.ReadKey().Key)
                {

                    case ConsoleKey.Enter:
                        if (!MyTree.DriversList[k].IsReady)  //якщо драйв не доступний
                        {

                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(PanelX + 22, PanelY + 12);
                            Console.WriteLine("Driver is INACCESSIBLE");
                            Thread.Sleep(2000);
                            Console.ResetColor();
                            ShowDrvPanel();
                            ShowDrvList(ref k);
                            continue;
                        }
                        else
                        {
                            MyTree.ActualDriver = MyTree.DriversList[k];
                            exit = true;
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        --k;
                        ShowDrvList(ref k);
                        break;

                    case ConsoleKey.DownArrow:
                        ++k;
                        ShowDrvList(ref k);
                        break;

                    default:
                        break;
                }
            }


        }
        public void ShowDrvPanel()
        {
            string[,] field = new string[10, 25];

            for (int i = 0; i < 10; i++)
            {

                for (int j = 0; j < 25; j++)
                {
                    field[i, j] = " ";
                }

            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    field[i, j] = "|";
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 24; j < 25; j++)
                {
                    field[i, j] = "|";
                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 10; j < 11; j++)
                {
                    field[i, j] = "|";
                }
            }
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    field[i, j] = "=";
                }
            }
            for (int i = 9; i < 10; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    field[i, j] = "=";
                }
            }



            int x = PanelX + 20, y = PanelY + 10;

            for (int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(x, y++);
                for (int j = 0; j < 25; j++)
                {

                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(field[i, j]);
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(x + 3, y - 10);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("NAME:");


            Console.SetCursorPosition(x + 13, y - 10);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("STATUS:");

            x = PanelX + 20;
            y = PanelY + 12;


            y = PanelY + 12;
            for (int i = 0; i < MyTree.DriversList.Length; i++)
            {
                Console.SetCursorPosition(x + 12, y++);
                if (MyTree.DriversList[i].IsReady)
                    Console.WriteLine("Ready");
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("INACCESSIBLE");
                    Console.ResetColor();
                }

            }


        }
        public void ShowDrvList(ref int selectIndex)
        {
            int x = PanelX + 22, y = PanelY + 12;
            if (selectIndex >= MyTree.DriversList.Length)
            {
                selectIndex = 0;
            }
            if (selectIndex < 0)
            {
                selectIndex = MyTree.DriversList.Length - 1;
            }

            for (int i = 0; i < MyTree.DriversList.Length; i++)
            {
                Console.SetCursorPosition(x, y++);
                if (i == selectIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(MyTree.DriversList[i].Name);
                    Console.ResetColor();
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(MyTree.DriversList[i].Name);
                    Console.ResetColor();
                }


            }
        }
        public void CreateDir()
        {

            string newDir = "";

            Console.SetCursorPosition(PanelX + 5, PanelY + 15);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("New directory:");


            for (int i = 0; i < 40; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write(" ");
            }
            Console.SetCursorPosition(PanelX + 20, PanelY + 15);
            bool complite = false;
            ConsoleKey enterChar;
            while (!complite)
            {

                enterChar = Console.ReadKey().Key;
                switch (enterChar)
                {
                    case ConsoleKey.Enter:
                        complite = true;
                        if (newDir.Length == 0)
                            newDir = "Directory";
                        break;
                    default:
                        if (newDir.Length > 38)
                        {
                            Console.SetCursorPosition(PanelX + 5, PanelY + 15);
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write("  Very long  ");
                            Thread.Sleep(2000);
                            Console.SetCursorPosition(PanelX + 5, PanelY + 15);
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write("New directory:");
                        }
                        else
                            newDir += (char)enterChar;

                        continue;

                }

            }
            string fullPath = "";
            for (int i = 0; i < MyTree.DriversList.Length; i++) //якщо створюємо в корні
            {
                if (MyTree.ActualeDirectory.FullName == MyTree.DriversList[i].Name)
                {
                    fullPath = MyTree.ActualeDirectory.FullName + newDir;
                    break;
                }
                else //якщо в папці
                {
                    fullPath = MyTree.ActualeDirectory.FullName + "\\" + newDir;
                    break;
                }
            }


            DirectoryInfo newDirectory = new DirectoryInfo(fullPath);
            if (!newDirectory.Exists)
            {
                newDirectory.Create();
            }
            else
            {
                Console.SetCursorPosition(PanelX + 5, PanelY + 15);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("                   Directory Exist!                     ");
                Thread.Sleep(2000);
            }

        }
        public void Delete(int delElement)
        {

            string delPath = MyTree.ActualeDirectory.FullName;
            if (MyTree.Type[delElement] == "<DIR>")
            {
                for (int i = 0; i < MyTree.DriversList.Length; i++) //якщо видаляєм в корні
                {
                    if (MyTree.ActualeDirectory.FullName == MyTree.DriversList[i].Name)
                    {
                        delPath = delPath + MyTree.Name[delElement];
                        break;
                    }
                    else //якщо в папці
                    {
                        delPath = delPath + "\\" + MyTree.Name[delElement];
                        break;
                    }
                }
                try
                {
                    if (MyTree.Name[delElement] == "..") //захист від ідіота
                    { }
                    else
                        Directory.Delete(delPath, false);

                }
                catch (Exception ex)
                {

                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(PanelX + 20, PanelY + 10);
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(1000);

                }

            }
            else
            {
                for (int i = 0; i < MyTree.DriversList.Length; i++) //якщо видаляєм в корні
                {
                    if (MyTree.ActualeDirectory.FullName == MyTree.DriversList[i].Name)
                    {
                        delPath += MyTree.Name[delElement];
                        break;
                    }
                    else //якщо в папці
                    {
                        delPath = delPath + "\\" + MyTree.Name[delElement];
                        break;
                    }
                }
                if (MyTree.Name[delElement] == "..") //захист від ідіота
                {
                }
                else
                {

                    File.Delete(delPath);
                }
            }

        }
        public void Rename()
        {
            if (MyTree.Name[MyTree.ActualeRecord] == "..")
            { }
            else
            {

                string newName = "";

                Console.SetCursorPosition(PanelX + 5, PanelY + 15);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("New name:");


                for (int i = 0; i < 40; i++)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Write(" ");
                }
                Console.SetCursorPosition(PanelX + 20, PanelY + 15);
                bool complite = false;
                ConsoleKey enterChar;
                while (!complite)
                {

                    enterChar = Console.ReadKey().Key;
                    switch (enterChar)
                    {
                        case ConsoleKey.Enter:
                            complite = true;
                            if (newName.Length == 0)
                                newName = "Directory";
                            break;
                        default:
                            if (newName.Length > 38)
                            {
                                Console.SetCursorPosition(PanelX + 5, PanelY + 15);
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.Write("  Very long  ");
                                Thread.Sleep(2000);
                                Console.SetCursorPosition(PanelX + 5, PanelY + 15);
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.Write("New directory:");
                            }
                            else
                                newName += (char)enterChar;

                            continue;

                    }


                }

                if (MyTree.Type[MyTree.ActualeRecord] != "<DIR>")
                    newName = newName + "." + MyTree.Type[MyTree.ActualeRecord].ToLower();

                string desDir, sourceDir;
                desDir = sourceDir = MyTree.ActualeDirectory.FullName;

                for (int i = 0; i < MyTree.DriversList.Length; i++) //якщо в корні
                {
                    if (MyTree.ActualeDirectory.FullName == MyTree.DriversList[i].Name)
                    {
                        sourceDir = sourceDir + MyTree.Name[MyTree.ActualeRecord];
                        desDir = desDir + newName;

                        break;
                    }
                    else //якщо в папці
                    {
                        sourceDir = sourceDir + "\\" + MyTree.Name[MyTree.ActualeRecord];
                        desDir = desDir + "\\" +
                            "" + newName;
                        break;
                    }
                }

                try
                {
                    Directory.Move(sourceDir, desDir);
                }
                catch (Exception ex)
                {

                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(50, 15);
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(1000);
                    Console.ResetColor();
                    Console.Clear();

                }


            }
        }

        public void View(int viewRecord)
        {
            
                string openF = MyTree.ActualeDirectory.FullName + "\\" + MyTree.Name[viewRecord];

                using (FileStream fstream = File.OpenRead(openF))
                {

                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                string File = Encoding.Default.GetString(array);
                Console.WriteLine(File);
                   
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("For Exit press \"q\"");
                Console.ResetColor();
            bool exit = false;
            while (!exit)
            {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Q:
                        exit = true;
                    break;
                default:
                    continue;
                 
            }
            }
            
            
            
        }
    }
}

using System;
using System.IO;
using System.Threading;

namespace MyFar_v1
{
    class Manager
    {
        Panel[] panel;
        public int actualPanel;
        public int actualString;

        public Manager()
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.ShowMenu();


            actualPanel = 0;
            actualString = 0;
            panel = new Panel[2];
            for (int i = 0; i < 2; i++)
                panel[i] = new Panel();
            panel[0].PanelX = 0;
            panel[1].PanelX = 70;
            panel[0].PanelY = panel[1].PanelY = 0;

            foreach (var item in panel)
            {
                item.ShowPanel();
            }

            panel[0].ShowSelect(2, 3);
            for (int i = 0; i < 2; i++)
            {
                if (i == actualPanel)
                    panel[i].MyTree.ShowPage(1, panel[i].PanelX, panel[i].PanelY, true);
                else
                    panel[i].MyTree.ShowPage(1, panel[i].PanelX, panel[i].PanelY, false);
            }

            int x, y;
            x = panel[actualPanel].PanelX + 2;
            y = panel[actualPanel].PanelY + 3;
            while (true)
            {

                switch (Console.ReadKey().Key)
                {

                    case ConsoleKey.DownArrow:
                        panel[actualPanel].MyTree.move = "down";
                        if (panel[actualPanel].MyTree.ActualeRecord > panel[actualPanel].MyTree.Name.Count - 2) //перевіряєм чи не падаєм нижче допустимого
                            break;
                        if (panel[actualPanel].MyTree.ActualeRecord > 28)// перевіряємо чи досягли кінця сторінки
                            panel[actualPanel].MyTree.ActualPage++;

                        if (panel[actualPanel].MyTree.ActualPage == 1)
                        {

                            panel[actualPanel].ShowSelect(x, ++y);
                            panel[actualPanel].CorectMoveDown(x, y);
                            panel[actualPanel].MyTree.ActualeRecord++;
                            panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);

                        }
                        else
                        {
                            panel[actualPanel].ShowSelect(x, y);
                            panel[actualPanel].MyTree.ActualeRecord++;
                            panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);

                            if (panel[actualPanel].MyTree.ActualeRecord % 30 == 1)//перевірям чи не пора збільшувати сторінку
                                panel[actualPanel].MyTree.ActualPage++;
                        }

                        break;
                    case ConsoleKey.UpArrow:  //
                        panel[actualPanel].MyTree.move = "up";
                        if (panel[actualPanel].MyTree.ActualeRecord == 0 || panel[actualPanel].MyTree.ActualeRecord > panel[actualPanel].MyTree.Name.Count) //перевіряєм чи не виходимо за рамки 
                            break;
                        if (panel[actualPanel].MyTree.ActualPage != 1 && panel[actualPanel].MyTree.ActualeRecord < 30) //перевіряєм чи не перейшли на сторіночку вверх
                            panel[actualPanel].MyTree.ActualPage--;

                        if (panel[actualPanel].MyTree.ActualPage == 1)
                        {
                            panel[actualPanel].ShowSelect(x, --y);
                            panel[actualPanel].CorectMoveUP(x, y);
                            panel[actualPanel].MyTree.ActualeRecord--;
                            panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);

                        }
                        else
                        {

                            panel[actualPanel].ShowSelect(x, y);
                            panel[actualPanel].MyTree.ActualeRecord--;
                            panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);

                        }

                        break;

                    case ConsoleKey.Tab:
                        actualPanel = 1 - actualPanel;

                        panel[1 - actualPanel].ShowPanel(); //перемалюєм не актуальну панель
                        panel[1 - actualPanel].MyTree.ShowPage(panel[1 - actualPanel].MyTree.ActualPage, panel[1 - actualPanel].PanelX, panel[1 - actualPanel].PanelY, false);

                        x = panel[actualPanel].PanelX + 2; //перекидаємо виділення на нову панель
                        y = panel[actualPanel].PanelY + 3;
                        panel[actualPanel].MyTree.ActualeRecord = 0;
                        panel[actualPanel].ShowSelect(panel[actualPanel].PanelX + 2, panel[actualPanel].PanelY + 3);
                        panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);
                        break;
                    case ConsoleKey.Enter:
                        try
                        {

                            if (panel[actualPanel].MyTree.ActualeRecord <= panel[actualPanel].MyTree.DirectoryList.Length)
                            {
                                if (panel[actualPanel].MyTree.Name[panel[actualPanel].MyTree.ActualeRecord] == "..") //піднімаємся вверх
                                {
                                    panel[actualPanel].MyTree.ActualeDirectory = panel[actualPanel].MyTree.ActualeDirectory.Parent;
                                }
                                else
                                {
                                    if (panel[actualPanel].MyTree.Name[0] == "..") //врахуємо що зсув може бути якщо ..
                                        panel[actualPanel].MyTree.ActualeDirectory = panel[actualPanel].MyTree.DirectoryList[panel[actualPanel].MyTree.ActualeRecord - 1];
                                    else
                                        panel[actualPanel].MyTree.ActualeDirectory = panel[actualPanel].MyTree.DirectoryList[panel[actualPanel].MyTree.ActualeRecord]; //заходимо в папку
                                }
                                panel[actualPanel].MyTree.CreatePages(); //формуємо нові сторінки

                                x = panel[actualPanel].PanelX + 2; //перекидаємо виділення на нову панель
                                y = panel[actualPanel].PanelY + 3;
                                panel[actualPanel].ShowPanel();
                                panel[actualPanel].ShowSelect(panel[actualPanel].PanelX + 2, panel[actualPanel].PanelY + 3);
                                panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);
                                panel[actualPanel].ShowPath();
                            }
                        }
                        catch (Exception ex)  //якщо доступ не дозволений
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(x + 10, y);
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(2000);
                            panel[actualPanel].ShowPanel();
                            panel[actualPanel].ShowSelect(x, y);
                            panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);
                        }

                        break;
                    case ConsoleKey.Backspace:  //test
                        try
                        {
                            panel[actualPanel].MyTree.ActualeDirectory = panel[actualPanel].MyTree.ActualeDirectory.Parent;

                            panel[actualPanel].MyTree.CreatePages(); //формуємо нові сторінки
                            x = panel[actualPanel].PanelX + 2; //перекидаємо виділення на нову панель
                            y = panel[actualPanel].PanelY + 3;
                            panel[actualPanel].ShowPanel();
                            panel[actualPanel].ShowSelect(panel[actualPanel].PanelX + 2, panel[actualPanel].PanelY + 3);
                            panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);
                        }
                        catch (Exception)  //ще один захист від ідіотів
                        {
                            continue;

                        }


                        break;
                    case ConsoleKey.D1:
                        panel[actualPanel].ChangeDriver();
                        panel[actualPanel].MyTree.ActualeDirectory = panel[actualPanel].MyTree.ActualDriver.RootDirectory; //test
                        panel[actualPanel].ShowPanel();
                        panel[actualPanel].MyTree.CreatePages();
                        x = panel[actualPanel].PanelX + 2; //перекидаємо виділення на нову панель
                        y = panel[actualPanel].PanelY + 3;
                        panel[actualPanel].MyTree.ActualeRecord = 0;
                        panel[actualPanel].ShowSelect(panel[actualPanel].PanelX + 2, panel[actualPanel].PanelY + 3);
                        panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);
                        break;
                    case ConsoleKey.D2:
                        if (panel[actualPanel].MyTree.Type[panel[actualPanel].MyTree.ActualeRecord]=="<DIR>")
                           break;
                        
                        Console.Clear();
                        panel[actualPanel].View(panel[actualPanel].MyTree.ActualeRecord);
                        
                        panel[actualPanel].ShowPanel();
                        panel[actualPanel].MyTree.CreatePages();
                        x = panel[actualPanel].PanelX + 2; 
                        y = panel[actualPanel].PanelY + 3;
                        panel[actualPanel].MyTree.ActualeRecord = 0;
                        panel[actualPanel].ShowSelect(panel[actualPanel].PanelX + 2, panel[actualPanel].PanelY + 3);
                        panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);
                        //
                        panel[1 - actualPanel].ShowPanel();
                        panel[1 - actualPanel].MyTree.CreatePages();
                        panel[1 - actualPanel].MyTree.ShowPage(panel[1 - actualPanel].MyTree.ActualPage, panel[1 - actualPanel].PanelX, panel[1 - actualPanel].PanelY, false);
                        mainMenu.ShowMenu();
                        break;

                    case ConsoleKey.D7:
                        panel[actualPanel].CreateDir(); //створюємо каталог
                        panel[actualPanel].ShowPanel();
                        panel[actualPanel].MyTree.CreatePages();
                        x = panel[actualPanel].PanelX + 2; //перекидаємо виділення на нову панель
                        y = panel[actualPanel].PanelY + 3;
                        panel[actualPanel].MyTree.ActualeRecord = 0;
                        panel[actualPanel].ShowSelect(panel[actualPanel].PanelX + 2, panel[actualPanel].PanelY + 3);
                        panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);
                        break;
                    case ConsoleKey.D3:
                        Move(panel[actualPanel].MyTree.ActualeRecord); //пеерміщаємо
                        panel[actualPanel].ShowPanel();
                        panel[actualPanel].MyTree.CreatePages();
                        x = panel[actualPanel].PanelX + 2; //перекидаємо виділення на нову панель
                        y = panel[actualPanel].PanelY + 3;
                        panel[actualPanel].MyTree.ActualeRecord = 0;
                        panel[actualPanel].ShowSelect(panel[actualPanel].PanelX + 2, panel[actualPanel].PanelY + 3);
                        panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);
                        //після переміщення перемалюєм і сусідню панель
                        panel[1 - actualPanel].ShowPanel();
                        panel[1 - actualPanel].MyTree.CreatePages();
                        panel[1 - actualPanel].MyTree.ShowPage(panel[1 - actualPanel].MyTree.ActualPage, panel[1 - actualPanel].PanelX, panel[1 - actualPanel].PanelY, false);
                        mainMenu.ShowMenu();
                        break;
                    case ConsoleKey.D4:
                        panel[actualPanel].Delete(panel[actualPanel].MyTree.ActualeRecord); //видаляєм
                        panel[actualPanel].ShowPanel();
                        panel[actualPanel].MyTree.CreatePages();
                        x = panel[actualPanel].PanelX + 2; //перекидаємо виділення на нову панель
                        y = panel[actualPanel].PanelY + 3;
                        panel[actualPanel].MyTree.ActualeRecord = 0;
                        panel[actualPanel].ShowSelect(panel[actualPanel].PanelX + 2, panel[actualPanel].PanelY + 3);
                        panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);

                        break;

                    case ConsoleKey.D5:
                        Copy(panel[actualPanel].MyTree.ActualeRecord); //Копіюємо
                        panel[actualPanel].ShowPanel();
                        panel[actualPanel].MyTree.CreatePages();
                        x = panel[actualPanel].PanelX + 2; //перекидаємо виділення на нову панель
                        y = panel[actualPanel].PanelY + 3;
                        panel[actualPanel].MyTree.ActualeRecord = 0;
                        panel[actualPanel].ShowSelect(panel[actualPanel].PanelX + 2, panel[actualPanel].PanelY + 3);
                        panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);
                        //після переміщення перемалюєм і сусідню панель
                        panel[1 - actualPanel].ShowPanel();
                        panel[1 - actualPanel].MyTree.CreatePages();
                        panel[1 - actualPanel].MyTree.ShowPage(panel[1 - actualPanel].MyTree.ActualPage, panel[1 - actualPanel].PanelX, panel[1 - actualPanel].PanelY, false);
                        mainMenu.ShowMenu();
                        break;

                    case ConsoleKey.D6:
                        panel[actualPanel].Rename(); //переіменовуємо
                        panel[actualPanel].ShowPanel();
                        panel[actualPanel].MyTree.CreatePages();
                        x = panel[actualPanel].PanelX + 2; //перекидаємо виділення на нову панель
                        y = panel[actualPanel].PanelY + 3;
                        panel[actualPanel].MyTree.ActualeRecord = 0;
                        panel[actualPanel].ShowSelect(panel[actualPanel].PanelX + 2, panel[actualPanel].PanelY + 3);
                        panel[actualPanel].MyTree.ShowPage(panel[actualPanel].MyTree.ActualPage, panel[actualPanel].PanelX, panel[actualPanel].PanelY, true);

                        break;

                    case ConsoleKey.D8:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.SetCursorPosition(0, 36);
                        Console.ResetColor();
                       break;
                        

                }
            }

        }
        public void Move(int moveElem)
        {
            string sourceDir = panel[actualPanel].MyTree.ActualeDirectory.FullName;
            string desDir = panel[1 - actualPanel].MyTree.ActualeDirectory.FullName;

            for (int i = 0; i < panel[actualPanel].MyTree.DriversList.Length; i++) //якщо в корні
            {
                if (panel[actualPanel].MyTree.ActualeDirectory.FullName == panel[actualPanel].MyTree.DriversList[i].Name)
                {
                    sourceDir = sourceDir + panel[actualPanel].MyTree.Name[moveElem];

                    break;
                }
                else //якщо в папці
                {
                    sourceDir = sourceDir + "\\" + panel[actualPanel].MyTree.Name[moveElem];
                    break;
                }
            }
            desDir = desDir + "\\" + panel[actualPanel].MyTree.Name[moveElem];
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
                Thread.Sleep(2000);
                Console.ResetColor();
                Console.Clear();

            }


        }
        public void Copy(int copyElem)
        {
            string sourceDir = panel[actualPanel].MyTree.ActualeDirectory.FullName;
            string desDir = panel[1 - actualPanel].MyTree.ActualeDirectory.FullName;

            for (int i = 0; i < panel[actualPanel].MyTree.DriversList.Length; i++) //якщо в корні
            {
                if (panel[actualPanel].MyTree.ActualeDirectory.FullName == panel[actualPanel].MyTree.DriversList[i].Name)
                {
                    sourceDir = sourceDir + panel[actualPanel].MyTree.Name[copyElem];

                    break;
                }
                else //якщо в папці
                {
                    sourceDir = sourceDir + "\\" + panel[actualPanel].MyTree.Name[copyElem];
                    break;
                }
            }
            desDir = desDir + "\\" + panel[actualPanel].MyTree.Name[copyElem];

            if (panel[actualPanel].MyTree.Type[copyElem] != "<DIR>") // для копіювання файлів
            {
                try
                {
                    File.Copy(sourceDir, desDir, true);
                }

                catch (Exception ex)
                {

                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(50, 15);
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(2000);
                    Console.ResetColor();
                    Console.Clear();

                }
            }
            else
            {
                if (sourceDir == desDir) //уникаємо перепису аналогічних папок
                { }
                else
                {
                    
                    if (panel[actualPanel].MyTree.ActualDriver.Name == panel[1 - actualPanel].MyTree.ActualDriver.Name)
                    {

                        //try
                        //{
                        //    Directory.Move(sourceDir, desDir);
                            
                        //}
                        //catch (Exception ex)
                        //{

                        //    Console.BackgroundColor = ConsoleColor.Red;
                        //    Console.ForegroundColor = ConsoleColor.Black;
                        //    Console.SetCursorPosition(50, 15);
                        //    Console.WriteLine(ex.Message);
                        //    Thread.Sleep(2000);
                        //    Console.ResetColor();
                        //    Console.Clear();

                        //}
                    }
                    else
                    { }
                }
            }


        }

    }
}

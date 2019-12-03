using System;
using System.Collections.Generic;
using System.IO;

namespace MyFar_v1
{
    class Tree
    {
        public DriveInfo[] DriversList { get; set; }
        public DirectoryInfo[] DirectoryList { get; set; }
        public FileInfo[] FileList { get; set; }
        public DriveInfo ActualDriver { set; get; }
        public DirectoryInfo ActualeDirectory { set; get; }
        public string Path { get; set; }
        public long FreeSpace { get; set; }
        public int Pages { get; set; }
        public int ActualPage { get; set; }
        public bool ChangePage { get; set; }
        public List<string> Name { get; set; }
        public List<string> Type { get; set; }
        public List<string> DateMod { get; set; }
        int startI = -1;
        int endI = -1;
        public string move; //напрям руху

        public int ActualeRecord { get; set; }
        public string ActualName { get; set; }

        public Tree()
        {
            DriversList = DriveInfo.GetDrives();
            for (int i = 0; i < DriversList.Length; i++)
            {
                if (DriversList[i].Name == "C:\\")
                    ActualDriver = DriversList[i]; //при створені актуальний диск С:
            }

            Path = "Select Drive";
            FreeSpace = 0;
            Name = new List<string>();
            Type = new List<string>();
            DateMod = new List<string>();
            ActualeDirectory = ActualDriver.RootDirectory;
            CreatePages();
            ChangePage = false;
            move = "down";
        }

        public void CreatePages()
        {
            
            DirectoryList = ActualeDirectory.GetDirectories();
            FileList = ActualeDirectory.GetFiles();
            if (Name.Count != 0) //коли заходимо в папку очищаємо списки
            {
                Name.Clear();
                Type.Clear();
                DateMod.Clear();
            }
            if (ActualeDirectory.Parent != null)
            {
                Name.Add("..");
                Type.Add("UP");
                DateMod.Add("\0");
            }



            // int i = 0;
            for (int i = 0; i < DirectoryList.Length; i++)
            {
                
                Name.Add(DirectoryList[i].Name);
                Type.Add("<DIR>");
                DateMod.Add((DirectoryList[i].LastWriteTime).ToShortDateString());
                
            }

            for (int i = 0; i < FileList.Length; i++)
            {
                
                Name.Add(FileList[i].Name);

                if (FileList[i].Extension.Length == 0)
                    Type.Add("\0");
                else
                    Type.Add((FileList[i].Extension).Remove(0, 1));

                DateMod.Add((FileList[i].LastWriteTime).ToShortDateString());
                
            }
           
            ActualeRecord = 0;

        }

        public void ShowPage(int pageNumb, int satrtX, int startY, bool actualPanel) //розбити вивід на page
        {
            
            Pages = Name.Count / 30 + 1;
            if (pageNumb > Pages)
                ActualPage = Pages;
            else
                ActualPage = pageNumb;

            int x = satrtX + 3, y = startY + 3;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;

            if (ActualPage == 1)
            {
                startI = 0;
                if (Name.Count < 30)
                    endI = Name.Count;
                else
                    endI = 30;
                for (int i = startI; i < endI; i++)
                {
                    Console.SetCursorPosition(x, y);
                    if (Name[i].Length > 29)  //якщо занадто довге слово
                    {
                        if (i == ActualeRecord && actualPanel == true)             //для актуального запису
                        {
                            ActualName = Name[ActualeRecord];
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            for (int j = 0; j < 28; j++)
                            {
                                Console.Write(Name[i][j]);
                            }
                            Console.Write("...");
                            Console.SetCursorPosition(x + 38, y); //виводимо тип
                            Console.WriteLine(Type[i]);
                            Console.SetCursorPosition(x + 51, y); //дату
                            Console.WriteLine(DateMod[i]);
                            Console.ResetColor();
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            for (int j = 0; j < 28; j++)
                            {
                                Console.Write(Name[i][j]);
                            }
                            Console.Write("...");
                            Console.SetCursorPosition(x + 38, y); //виводимо тип
                            Console.WriteLine(Type[i]);
                            Console.SetCursorPosition(x + 51, y); //дату
                            Console.WriteLine(DateMod[i]);
                        }
                    }
                    else
                    {
                        if (i == ActualeRecord && actualPanel == true)             //для актуального запису
                        {
                            ActualName = Name[ActualeRecord];
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine(Name[i]);
                            Console.SetCursorPosition(x + 38, y); //виводимо тип

                            Console.WriteLine(Type[i]);
                            Console.SetCursorPosition(x + 51, y); //дату
                            Console.WriteLine(DateMod[i]);
                            Console.ResetColor();
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.WriteLine(Name[i]);
                            Console.SetCursorPosition(x + 38, y); //виводимо тип
                            Console.WriteLine(Type[i]);
                            Console.SetCursorPosition(x + 51, y); //дату
                            Console.WriteLine(DateMod[i]);
                        }

                    }
                    y++;
                }
            }
            else
            {
                x = satrtX + 3; y = startY + 3;
                int corectI = startI;
                for (int i = 0; i < 30; i++)
                {

                    Console.SetCursorPosition(x, y); 
                    for (int j = 0; j < Name[corectI].Length; j++)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.Write("\0");
                    }
                    Console.SetCursorPosition(x + 38, y);
                    
                        for (int j = 0; j < Type[corectI].Length; j++)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.Write("\0");
                        }
                    
                    
                    Console.SetCursorPosition(x + 51, y);
                    for (int j = 0; j < DateMod[corectI].Length; j++)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.Write("\0");
                    }
                    corectI++;
                    y++;
                    Console.WriteLine();
                }

                x = satrtX + 3; y = startY + 3;
                if (move == "down")  //напрямок руху визначає межі пейджинга
                {
                    startI++;
                    endI++;
                }
                else
                {
                    startI--;
                    endI--;
                }
                
                for (int i = startI; i < endI; i++)
                {
                    Console.SetCursorPosition(x, y);
                    if (Name[i].Length > 29)  //якщо занадто довге слово
                    {
                        if (i == ActualeRecord && actualPanel == true)             //для актуального запису
                        {
                            ActualName = Name[ActualeRecord];
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            for (int j = 0; j < 28; j++)
                            {
                                Console.Write(Name[i][j]);
                            }
                            Console.Write("...");
                            Console.SetCursorPosition(x + 38, y); //виводимо тип
                            Console.WriteLine(Type[i]);
                            Console.SetCursorPosition(x + 51, y); //дату
                            Console.WriteLine(DateMod[i]);
                            Console.ResetColor();
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            for (int j = 0; j < 28; j++)
                            {
                                Console.Write(Name[i][j]);
                            }
                            Console.Write("...");
                            Console.SetCursorPosition(x + 38, y); //виводимо тип
                            Console.WriteLine(Type[i]);
                            Console.SetCursorPosition(x + 51, y); //дату
                            Console.WriteLine(DateMod[i]);
                        }
                    }
                    else
                    {
                        if (i == ActualeRecord && actualPanel == true)             //для актуального запису
                        {
                            ActualName = Name[ActualeRecord];
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine(Name[i]);
                            Console.SetCursorPosition(x + 38, y); //виводимо тип

                            Console.WriteLine(Type[i]);
                            Console.SetCursorPosition(x + 51, y); //дату
                            Console.WriteLine(DateMod[i]);
                            Console.ResetColor();
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.WriteLine(Name[i]);
                            Console.SetCursorPosition(x + 38, y); //виводимо тип
                            Console.WriteLine(Type[i]);
                            Console.SetCursorPosition(x + 51, y); //дату
                            Console.WriteLine(DateMod[i]);
                        }
                    }
                    y++;
                }

            }
            //  int x = satrtX + 3, y = startY + 3;
            //Console.BackgroundColor = ConsoleColor.DarkCyan;
            //Console.ForegroundColor = ConsoleColor.Black;
            // for (int i = 0; i < Name.Count; i++)
            //{
            //    Console.SetCursorPosition(x, y);
            //    if (i < 30)
            //    {
            //        if (Name[i].Length > 30)  //якщо занадто довге слово
            //        {
            //            if (i == ActualeRecord && actualPanel == true)             //для актуального запису
            //            {
            //                ActualName = Name[ActualeRecord];
            //                Console.BackgroundColor = ConsoleColor.White;
            //                Console.ForegroundColor = ConsoleColor.Black;
            //                for (int j = 0; j < 29; j++)
            //                {
            //                    Console.Write(Name[i][j]);
            //                }
            //                Console.Write("...");
            //                Console.ResetColor();
            //                Console.BackgroundColor = ConsoleColor.DarkCyan;
            //                Console.ForegroundColor = ConsoleColor.Black;
            //            }
            //            else
            //            {
            //                for (int j = 0; j < 29; j++)
            //                {
            //                    Console.Write(Name[i][j]);
            //                }
            //                Console.Write("...");
            //            }
            //        }
            //        else
            //        {
            //            if (i == ActualeRecord && actualPanel == true)             //для актуального запису
            //            {
            //                ActualName = Name[ActualeRecord];
            //                Console.BackgroundColor = ConsoleColor.White;
            //                Console.ForegroundColor = ConsoleColor.Black;
            //                Console.WriteLine(Name[i]);
            //                Console.ResetColor();
            //                Console.BackgroundColor = ConsoleColor.DarkCyan;
            //                Console.ForegroundColor = ConsoleColor.Black;
            //            }
            //            else
            //                Console.WriteLine(Name[i]);
            //        }
            //    }
            //    else
            //    {

            //    }

            //    y++;
            //}
            //if (Pages == 1 /*|| pageNumb==Pages*/)
            //{

            //    for (int i = 0; i < Name.Count; i++)
            //    {
            //        Console.SetCursorPosition(x + 3, y + 3);
            //        if (Name[i].Length > 30)  //якщо занадто довге слово
            //        {
            //            if (i == ActualeRecord && actualPanel == true)             //для актуального запису
            //            {
            //                ActualName = Name[ActualeRecord];
            //                Console.BackgroundColor = ConsoleColor.White;
            //                Console.ForegroundColor = ConsoleColor.Black;
            //                for (int j = 0; j < 29; j++)
            //                {
            //                    Console.Write(Name[i][j]);
            //                }
            //                Console.Write("...");
            //                Console.ResetColor();
            //                Console.BackgroundColor = ConsoleColor.DarkCyan;
            //                Console.ForegroundColor = ConsoleColor.Black;
            //            }
            //            else
            //            {
            //                for (int j = 0; j < 29; j++)
            //                {
            //                    Console.Write(Name[i][j]);
            //                }
            //                Console.Write("...");
            //            }
            //        }
            //        else
            //        {
            //            if (i == ActualeRecord && actualPanel == true)             //для актуального запису
            //            {
            //                ActualName = Name[ActualeRecord];
            //                Console.BackgroundColor = ConsoleColor.White;
            //                Console.ForegroundColor = ConsoleColor.Black;
            //                Console.WriteLine(Name[i]);
            //                Console.ResetColor();
            //                Console.BackgroundColor = ConsoleColor.DarkCyan;
            //                Console.ForegroundColor = ConsoleColor.Black;
            //            }
            //            else
            //                Console.WriteLine(Name[i]);
            //        }

            //        y++;
            //    }
            //    y = startY;
            //    for (int i = 0; i < Type.Count; i++)
            //    {

            //        Console.SetCursorPosition(x + 40, y + 3);

            //        if (i == ActualeRecord && actualPanel == true)             //для актуального запису
            //        {
            //            ActualName = Name[ActualeRecord];
            //            Console.BackgroundColor = ConsoleColor.White;
            //            Console.ForegroundColor = ConsoleColor.Black;
            //            Console.WriteLine(Type[i]);
            //            Console.ResetColor();
            //            Console.BackgroundColor = ConsoleColor.DarkCyan;
            //            Console.ForegroundColor = ConsoleColor.Black;
            //        }
            //        else
            //            Console.WriteLine(Type[i]);
            //        y++;
            //    }
            //    y = startY;
            //    for (int i = 0; i < DateMod.Count; i++)
            //    {
            //        Console.SetCursorPosition(x + 55, y + 3);
            //        if (i == ActualeRecord && actualPanel == true)             //для актуального запису
            //        {
            //            ActualName = Name[ActualeRecord];
            //            Console.BackgroundColor = ConsoleColor.White;
            //            Console.ForegroundColor = ConsoleColor.Black;
            //            Console.WriteLine(DateMod[i]);
            //            Console.ResetColor();
            //            Console.BackgroundColor = ConsoleColor.DarkCyan;
            //            Console.ForegroundColor = ConsoleColor.Black;
            //        }
            //        else
            //            Console.WriteLine(DateMod[i]);
            //        y++;
            //    }

            //}
            //else
            //{
            //    int startIndex = 30 * (pageNumb - 1);
            //    int lastIndex = startIndex + 30;
            //    if (lastIndex > (DirectoryList.Length + FileList.Length))
            //        lastIndex = (DirectoryList.Length + FileList.Length);
            //    for (int i = startIndex; i < lastIndex; i++)
            //    {
            //        Console.BackgroundColor = ConsoleColor.DarkCyan;
            //        Console.ForegroundColor = ConsoleColor.Black;

            //        if (ActualPage > 1 && ChangePage == true)
            //        {
            //            ActualeRecord++; //корекція при переході на нову сторінку
            //            ChangePage = false;
            //        }
            //        if (i == ActualeRecord && actualPanel == true)             //для актуального запису
            //        {
            //            ActualName = Name[ActualeRecord];
            //            Console.BackgroundColor = ConsoleColor.White;
            //            Console.ForegroundColor = ConsoleColor.Black;
            //        }


            //        Console.SetCursorPosition(x + 3, y + 3);
            //        if (Name[i].Length > 30)
            //        {
            //            for (int j = 0; j < 29; j++)
            //            {
            //                Console.Write(Name[i][j]);
            //            }
            //            Console.Write("...");
            //        }
            //        else
            //            Console.WriteLine(Name[i]);

            //        Console.SetCursorPosition(x + 40, y + 3);
            //        Console.WriteLine(Type[i]);
            //        Console.SetCursorPosition(x + 55, y + 3);
            //        Console.WriteLine(DateMod[i]);
            //        y++;
            //        Console.ResetColor();
            //    }




            //}

            Console.ResetColor();
        }

        public void ShowTree(int startIndex, int endIndex)
        {
            if (ActualeRecord > 30)
                ActualPage++;


        }



    }
}

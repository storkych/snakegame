using System;

class Program
{
    static void Main()
    {
        ConsoleKeyInfo keyInfo;
        int selectedItem = 0;
        bool isPlaying = false;

        Console.CursorVisible = false;
        Console.BackgroundColor = ConsoleColor.Black;

        while (true)
        {
            Console.Clear();

            if (isPlaying)
            {
                // Игровое поле
                DrawGameArea();
                // Здесь вы можете добавить логику для змейки и других элементов игры

                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isPlaying = false;
                }
            }
            else
            {
                string[] menuItems = { "Продолжить игру", "Новая игра", "Таблица рекордов", "Выход" };

                // Вывод меню с выделением выбранного пункта
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == selectedItem)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    Console.WriteLine((i == selectedItem ? ">> " : "   ") + menuItems[i]);
                }

                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.W && selectedItem > 0)
                {
                    selectedItem--;
                }
                else if (keyInfo.Key == ConsoleKey.S && selectedItem < menuItems.Length - 1)
                {
                    selectedItem++;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (selectedItem == 3)
                    {
                        // Если выбран "Выход", завершаем программу
                        break;
                    }
                    else if (selectedItem == 1)
                    {
                        // Если выбран "Новая игра", начинаем игру
                        isPlaying = true;
                    }
                    // Здесь можно добавить обработку других пунктов меню
                }
            }
        }
    }

    static void DrawGameArea()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // Установить кодировку для отображения символов Unicode

        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;

        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        Console.BackgroundColor = ConsoleColor.Red;
        for (int i = 0; i < screenWidth; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("█");
            Console.SetCursorPosition(i, screenHeight - 1);
            Console.Write("█");
        }

        for (int i = 0; i < screenHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("█");
            Console.SetCursorPosition(screenWidth - 1, i);
            Console.Write("█");
        }
    }



}

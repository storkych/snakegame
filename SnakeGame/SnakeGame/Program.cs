using System;

class Program
{
    static void Main()
    {
        ConsoleKeyInfo keyInfo;
        int selectedItem = 0;
        string[] menuItems = { "Продолжить игру", "Новая игра", "Таблица рекордов", "Выход" };

        while (true)
        {
            Console.Clear();

            // Вывод меню с выделением выбранного пункта
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedItem)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
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
                // Обработка выбранного пункта меню
                Console.Clear();
                Console.WriteLine("Выбран пункт: " + menuItems[selectedItem]);

                if (selectedItem == menuItems.Length - 1)
                {
                    // Если выбран "Выход", завершаем программу
                    break;
                }
                // обработка пунктов
                Console.ReadKey();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace SnakeGame
{
    class Program
    {
        private const int MapWidth = 30;
        private const int MapHeight = 20;
        private const int MaxRecords = 10;
        private const string RecordsFileName = "records.txt";

        private const int ScreenWidth = MapWidth * 3;
        private const int ScreenHeight = MapHeight * 3;

        private const int FrameMilliseconds = 200;

        private const ConsoleColor BorderColor = ConsoleColor.White;
        private const ConsoleColor FoodColor = ConsoleColor.Green;
        private const ConsoleColor BodyColor = ConsoleColor.White;
        private const ConsoleColor HeadColor = ConsoleColor.DarkGray;

        private static readonly Random Random = new Random();

        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private static Task inputTask;

        public static int GameState = 0; // 0 - Меню, 1 - Игра, 2 - экран смерти, 3 - экран паузы

        static async Task Main(string[] args)
        {
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {
                Task inputTask = HandleInputAsync(cancellationTokenSource.Token);
                bool isPlaying = false;
                Direction snakeDirection = Direction.Right;
                bool exitRequested = false;

                SetWindowSize(ScreenWidth, ScreenHeight);
                SetBufferSize(ScreenWidth, ScreenHeight);
                CursorVisible = false;

                List<string> records = ReadRecords(RecordsFileName);
                records.Sort((a, b) => int.Parse(b.Split(' ')[1]) - int.Parse(a.Split(' ')[1]));

                ConsoleKeyInfo keyInfo;
                int selectedItem = 0;

                while (!exitRequested)
                {
                    Clear();

                    if (isPlaying)
                    {
                        string playerName = GetPlayerName();
                        int score = StartGame();
                        records.Add($"{playerName} {score}");
                        records.Sort((a, b) => int.Parse(b.Split(' ')[1]) - int.Parse(a.Split(' ')[1]));

                        if (records.Count > MaxRecords)
                        {
                            records.RemoveAt(records.Count - 1);
                        }

                        WriteRecords(RecordsFileName, records);

                        isPlaying = false;
                    }
                    else
                    {
                        string[] menuItems = { "Продолжить", "Новая игра", "Таблица рекордов", "Выход" };

                        for (int i = 0; i < menuItems.Length; i++)
                        {
                            if (i == selectedItem)
                            {
                                ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                ForegroundColor = ConsoleColor.Gray;
                            }

                            WriteLine((i == selectedItem ? ">> " : "   ") + menuItems[i]);
                        }

                        keyInfo = ReadKey(true);

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
                                exitRequested = true; // Устанавливаем флаг выхода
                                cancellationTokenSource.Cancel(); // Отменяем ввод
                            }
                            else if (selectedItem == 1)
                            {
                                isPlaying = true;
                            }
                            else if (selectedItem == 2)
                            {
                                ShowRecords(records);
                                selectedItem = 0;
                            }
                        }
                    }

                    await Task.Delay(1); // Добавляем задержку
                }

                // Ожидаем завершения обработки ввода
                await inputTask;
            }
        }




        private static Direction SnakeDir = Direction.Right;

        static Direction ReadMovement()
        {
            return SnakeDir;
        }

        static async Task HandleInputAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        cancellationToken.ThrowIfCancellationRequested(); // Выбрасывает исключение при отмене
                    }
                    else if (key == ConsoleKey.W)
                    {
                        if (SnakeDir != Direction.Down) { SnakeDir = Direction.Up; }
                    }
                    else if (key == ConsoleKey.S)
                    {
                        if (SnakeDir != Direction.Up) { SnakeDir = Direction.Down; }
                    }
                    else if (key == ConsoleKey.A)
                    {
                        if (SnakeDir != Direction.Right) { SnakeDir = Direction.Left; }
                    }
                    else if (key == ConsoleKey.D)
                    {
                        if (SnakeDir != Direction.Left) { SnakeDir = Direction.Right; }
                    }
                }
                await Task.Delay(10);
            }
        }


        static int StartGame()
        {
            int score = 0;
            bool isGameOver = false;

            Clear();
            DrawBoard();

            Snake snake = new Snake(10, 5, HeadColor, BodyColor);

            Pixel food = GenFood(snake);
            food.Draw();

            Direction currentMovement = Direction.Right;


            int lagMs = 0;
            var sw = new Stopwatch();

            while (!isGameOver)
            {
                sw.Restart();
                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMilliseconds - lagMs)
                {
                    if (currentMovement == oldMovement)
                    {
                        currentMovement = ReadMovement();
                    }
                }

                sw.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);
                    food = GenFood(snake);
                    food.Draw();

                    score++;
                }
                else
                {
                    snake.Move(currentMovement);
                }

                if (snake.Head.X == MapWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MapHeight - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                {
                    isGameOver = true;
                }

                lagMs = (int)sw.ElapsedMilliseconds;
            }

            snake.Clear();
            food.Clear();

            for (int i = 1; i < MapWidth - 1; i++)
            {
                for (int j = 1; j < MapHeight - 1; j++)
                {
                    SetCursorPosition(i * 3, j);
                    Write(" ");
                }
            }

            ShowGameOver(score);

            while (true)
            {
                var keyInfo = ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                    break;
            }

            return score;
        }

        static void ShowGameOver(int score)
        {
            Clear();

            string[] gameOverText = new string[]
            {
                "  ▄▄ •  ▄▄▄· • ▌ ▄ ·. ▄▄▄ .         ▌ ▐·▄▄▄ .▄▄▄  ",
                " ▐█ ▀ ▪▐█ ▀█ ·██ ▐███▪▀▄.▀·   ▄█▀▄ ▪█·█▌▀▄.▀·▀▄ █·",
                " ▄█ ▀█▄▄█▀▀█ ▐█ ▌▐▌▐█·▐▀▀▪▄  ▐█▌.▐▌▐█▐█•▐▀▀▪▄▐▀▀▄ ",
                " ▐█▄▪▐█▐█▪ ▐▌██ ██▌▐█▌▐█▄▄▌  ▐█▌.▐▌ ███ ▐█▄▄▌▐█•█▌",
                "  ·▀▀▀▀  ▀  ▀ ▀▀  █▪▀▀▀ ▀▀▀    ▀█▄▀▪. ▀   ▀▀▀ .▀  ▀"
            };

            for (int i = 0; i < gameOverText.Length; i++)
            {
                SetCursorPosition(1, i);
                WriteLine(gameOverText[i]);
            }

            SetCursorPosition(1, gameOverText.Length + 2);
            WriteLine($"Your score: {score}");

            SetCursorPosition(1, gameOverText.Length + 4);
            WriteLine("Press Enter to return to the main menu.");
        }

        static string GetPlayerName()
        {
            Clear();

            string playerName = "";
            Write("Введите ваш ник: ");
            playerName = ReadLine();

            return playerName;
        }

        static void DrawBoard()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw();
                new Pixel(i, MapHeight - 1, BorderColor).Draw();
            }

            for (int i = 0; i < MapHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw();
                new Pixel(MapWidth - 1, i, BorderColor).Draw();
            }
        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;

            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHeight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y ||
                     snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }



        static List<string> ReadRecords(string fileName)
        {
            List<string> records = new List<string>();

            if (File.Exists(fileName))
            {
                records = File.ReadAllLines(fileName).ToList();
            }

            return records;
        }

        static void WriteRecords(string fileName, List<string> records)
        {
            File.WriteAllLines(fileName, records);
        }

        static void ShowRecords(List<string> records)
        {
            Clear();

            WriteLine("Таблица рекордов:");

            for (int i = 0; i < records.Count; i++)
            {
                if (i >= MaxRecords)
                {
                    break;
                }

                string[] record = records[i].Split(' ');
                WriteLine($"{i + 1}. {record[0]}: {record[1]}");
            }

            WriteLine("\nНажмите Enter, чтобы вернуться в главное меню.");
            ReadKey();
        }
    }

}

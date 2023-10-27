using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Zmeika2
{
    class Program
    {
        private const int MapWidth = 30;
        private const int MapHeight = 20;

        private const int ScreenWidth = MapWidth * 3;
        private const int ScreenHeight = MapHeight * 3;

        private const int FrameMilliseconds = 200;

        private const ConsoleColor BorderColor = ConsoleColor.White;
        private const ConsoleColor FoodColor = ConsoleColor.Green;
        private const ConsoleColor BodyColor = ConsoleColor.White;
        private const ConsoleColor HeadColor = ConsoleColor.DarkGray;

        private static readonly Random Random = new Random();


        static void Main()
        {
            ConsoleKeyInfo keyInfo;
            int selectedItem = 0;
            bool isPlaying = false;

            SetWindowSize(ScreenWidth, ScreenHeight);
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible = false;

            while (true)
            {
                Clear();

                if (isPlaying)
                {
                    string playerName = GetPlayerName();
                    StartGame(playerName);
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
                            break;
                        }
                        else if (selectedItem == 1)
                        {
                            isPlaying = true;
                        }
                        else if (selectedItem == 2)
                        {
                            DisplayLeaderboard();
                        }
                    }
                }
            }
        }

        static string GetPlayerName()
        {
            Console.Clear();
            Console.Write("Введите свое имя: ");
            return Console.ReadLine();
        }

        static void StartGame(string playerName)
        {
            int score = 0;
            bool isGameOver = false;
            bool isPaused = false;

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
                        if (!isPaused)
                        {
                            currentMovement = ReadMovement(currentMovement);
                        }
                    }

                    if (KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = ReadKey(true);

                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            if (isPaused)
                            {
                                isPaused = false;
                                DrawBoard();
                            }
                            else
                            {
                                isGameOver = true;
                            }
                        }
                        else if (keyInfo.Key == ConsoleKey.P)
                        {
                            isPaused = !isPaused;
                        }
                    }
                }

                if (isPaused)
                {
                    continue; // Игра на паузе, пропустить кадр
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

            string[] title = new string[]
            {
        "  ▄▄ •  ▄▄▄· • ▌ ▄ ·. ▄▄▄ .         ▌ ▐·▄▄▄ .▄▄▄  ",
        " ▐█ ▀ ▪▐█ ▀█ ·██ ▐███▪▀▄.▀·   ▄█▀▄ ▪█·█▌▀▄.▀·▀▄ █·",
        " ▄█ ▀█▄▄█▀▀█ ▐█ ▌▐▌▐█·▐▀▀▪▄  ▐█▌.▐▌▐█▐█•▐▀▀▪▄▐▀▀▄ ",
        " ▐█▄▪▐█▐█▪ ▐▌██ ██▌▐█▌▐█▄▄▌  ▐█▌.▐▌ ███ ▐█▄▄▌▐█•█▌",
        "  ·▀▀▀▀  ▀  ▀ ▀▀  █▪▀▀▀ ▀▀▀    ▀█▄▀▪. ▀   ▀▀▀ .▀  ▀"
            };

            for (int i = 0; i < title.Length; i++)
            {
                SetCursorPosition(1, i);
                WriteLine(title[i]);
            }

            SetCursorPosition(1, title.Length);
            WriteLine($"Score: {score}");

            SavePlayerResult(playerName, score);

            SetCursorPosition(1, title.Length + 2);
            WriteLine("Press Enter to return to the main menu.");

            while (true)
            {
                var keyInfo = ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                    break;
            }
        }

        static void SavePlayerResult(string playerName, int score)
        {
            string playerData = $"{playerName}: {score}";
            File.AppendAllLines("leaderboard.txt", new[] { playerData });
        }

        static void DisplayLeaderboard()
        {
            Console.Clear();
            Console.WriteLine("Таблица рекордов:");

            // Прочитать данные из файла и вывести на экран
            if (File.Exists("leaderboard.txt"))
            {
                string[] leaderboardData = File.ReadAllLines("leaderboard.txt");
                var leaderboard = new List<KeyValuePair<string, int>>();

                foreach (var entry in leaderboardData)
                {
                    string[] parts = entry.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int score))
                    {
                        leaderboard.Add(new KeyValuePair<string, int>(parts[0].Trim(), score));
                    }
                }

                // Сортируем таблицу рекордов
                leaderboard.Sort((a, b) => b.Value.CompareTo(a.Value));

                int rank = 1;
                foreach (var entry in leaderboard.Take(10))
                {
                    Console.WriteLine($"{rank}. {entry.Key}: {entry.Value}");
                    rank++;
                }
            }

            Console.WriteLine("\nНажмите Enter, чтобы вернуться в главное меню.");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
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

        static Direction ReadMovement(Direction currentDirection)
        {
            if (!KeyAvailable)
                return currentDirection;

            ConsoleKeyInfo keyInfo = ReadKey(true);
            ConsoleKey key = keyInfo.Key;

            if (key == ConsoleKey.W && currentDirection != Direction.Down)
                return Direction.Up;
            if (key == ConsoleKey.S && currentDirection != Direction.Up)
                return Direction.Down;
            if (key == ConsoleKey.A && currentDirection != Direction.Right)
                return Direction.Left;
            if (key == ConsoleKey.D && currentDirection != Direction.Left)
                return Direction.Right;

            return currentDirection;
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public readonly struct Pixel
    {
        private const char PixelChar = '█';

        public Pixel(int x, int y, ConsoleColor color, int pixelSize = 3)
        {
            X = x;
            Y = y;
            Color = color;
            PixelSize = pixelSize;
        }

        public int X { get; }

        public int Y { get; }

        public ConsoleColor Color { get; }

        public int PixelSize { get; }

        public void Draw()
        {
            ForegroundColor = Color;
            for (int x = 0; x < PixelSize; x++)
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Write(PixelChar);
                }
            }
        }

        public void Clear()
        {
            for (int x = 0; x < PixelSize; x++)
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Write(' ');
                }
            }
        }
    }

    public class Snake
    {
        private readonly ConsoleColor _headColor;
        private readonly ConsoleColor _bodyColor;

        public Snake(int initialX,
            int initialY,
            ConsoleColor headColor,
            ConsoleColor bodyColor,
            int bodyLength = 3)
        {
            _headColor = headColor;
            _bodyColor = bodyColor;

            Head = new Pixel(initialX, initialY, headColor);

            for (int i = bodyLength; i >= 0; i--)
            {
                Body.Enqueue(new Pixel(Head.X - i - 1, initialY, _bodyColor));
            }

            Draw();
        }

        public Pixel Head { get; private set; }
        public Queue<Pixel> Body { get; } = new Queue<Pixel>();

        public void Move(Direction direction, bool eat = false)
        {
            Clear();

            Body.Enqueue(new Pixel(Head.X, Head.Y, _bodyColor));
            if (!eat)
                Body.Dequeue();

            if (direction == Direction.Right)
            {
                Head = new Pixel(Head.X + 1, Head.Y, _headColor);
            }
            else if (direction == Direction.Left)
            {
                Head = new Pixel(Head.X - 1, Head.Y, _headColor);
            }
            else if (direction == Direction.Up)
            {
                Head = new Pixel(Head.X, Head.Y - 1, _headColor);
            }
            else if (direction == Direction.Down)
            {
                Head = new Pixel(Head.X, Head.Y + 1, _headColor);
            }

            Draw();
        }

        public void Draw()
        {
            Head.Draw();

            foreach (Pixel pixel in Body)
            {
                pixel.Draw();
            }
        }

        public void Clear()
        {
            Head.Clear();

            foreach (Pixel pixel in Body)
            {
                pixel.Clear();
            }
        }
    }
}

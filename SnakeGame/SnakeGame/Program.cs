using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    StartGame();
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
                    }
                }
            }
        }

        static void StartGame()
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

            int topOffset = 7;

            int bottomRow = ScreenHeight - 1 - topOffset;

            while (!isGameOver)
            {
                sw.Restart();
                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMilliseconds - lagMs)
                {
                    if (currentMovement == oldMovement)
                        currentMovement = ReadMovement(currentMovement);
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
                SetCursorPosition((ScreenWidth - title[i].Length) / 2, i + topOffset);
                WriteLine(title[i]);
            }

            SetCursorPosition((ScreenWidth - 16) / 2, topOffset + title.Length + 1);
            WriteLine($"Score: {score}");

            SetCursorPosition((ScreenWidth - 32) / 2, topOffset + title.Length + 3);
            WriteLine("Press Enter to return to the main menu.");

            while (true)
            {
                var keyInfo = ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                    break;
            }
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

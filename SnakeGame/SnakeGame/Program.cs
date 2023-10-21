using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.CursorVisible = false;
        Console.BackgroundColor = ConsoleColor.Black;
        bool isPlaying = false;

        while (true)
        {
            Console.Clear();

            if (isPlaying)
            {
                // Игровое поле

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isPlaying = false;
                }
            }
            else
            {
                string[] menuItems = { "1. Начать игру", "2. Выйти" };
                int selectedItem = 0;

                while (true)
                {
                    Console.Clear();

                    for (int i = 0; i < menuItems.Length; i++)
                    {
                        Console.ForegroundColor = i == selectedItem ? ConsoleColor.White : ConsoleColor.Gray;
                        Console.WriteLine(i == selectedItem ? ">> " + menuItems[i] : "   " + menuItems[i]);
                    }

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

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
                        if (selectedItem == 0)
                        {
                            StartGame();
                            isPlaying = true;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }
    }

    static void StartGame()
    {
        int width = 80;
        int height = 40;

        SnakeGame game = new SnakeGame(width, height);
        game.Run();

        Console.Clear();
        Console.WriteLine("  ▄▄ •  ▄▄▄· • ▌ ▄ ·. ▄▄▄ .         ▌ ▐·▄▄▄ .▄▄▄  ");
        Console.WriteLine(" ▐█ ▀ ▪▐█ ▀█ ·██ ▐███▪▀▄.▀·   ▄█▀▄ ▪█·█▌▀▄.▀·▀▄ █·");
        Console.WriteLine(" ▄█ ▀█▄▄█▀▀█ ▐█ ▌▐▌▐█·▐▀▀▪▄  ▐█▌.▐▌▐█▐█•▐▀▀▪▄▐▀▀▄ ");
        Console.WriteLine(" ▐█▄▪▐█▐█▪ ▐▌██ ██▌▐█▌▐█▄▄▌  ▐█▌.▐▌ ███ ▐█▄▄▌▐█•█▌");
        Console.WriteLine("  ·▀▀▀▀  ▀  ▀ ▀▀  █▪▀▀▀ ▀▀▀    ▀█▄▀▪. ▀   ▀▀▀ .▀  ▀");
        Console.WriteLine("\nИгра завершена. Ваш счет: " + game.Score);

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                return;
            }
        }
    }
}

class SnakeGame
{
    private int width;
    private int height;
    private int snakeX;
    private int snakeY;
    private int foodX;
    private int foodY;
    private List<int> tailX;
    private List<int> tailY;
    private int score;
    private bool isGameOver;
    private Direction direction;

    public int Score { get; private set; }

    public SnakeGame(int width, int height)
    {
        this.width = width;
        this.height = height;
        snakeX = width / 2;
        snakeY = height / 2;
        tailX = new List<int>();
        tailY = new List<int>();
        foodX = new Random().Next(1, width - 1);
        foodY = new Random().Next(1, height - 1);
        score = 0;
        isGameOver = false;
        direction = Direction.Right;
        Console.CursorVisible = false;
    }

    public void Run()
    {
        while (!isGameOver)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                ProcessKey(key);
            }

            MoveSnake();
            if (snakeX == foodX && snakeY == foodY)
            {
                EatFood();
                GenerateFood();
            }

            if (snakeX == 0 || snakeX == width - 1 || snakeY == 0 || snakeY == height - 1 || CheckCollisionWithTail())
            {
                isGameOver = true;
            }

            DrawGame();
            Thread.Sleep(100);
        }
    }

    private void ProcessKey(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.W:
                if (direction != Direction.Down)
                    direction = Direction.Up;
                break;
            case ConsoleKey.S:
                if (direction != Direction.Up)
                    direction = Direction.Down;
                break;
            case ConsoleKey.A:
                if (direction != Direction.Right)
                    direction = Direction.Left;
                break;
            case ConsoleKey.D:
                if (direction != Direction.Left)
                    direction = Direction.Right;
                break;
        }
    }

    private void MoveSnake()
    {
        int prevX = snakeX;
        int prevY = snakeY;

        for (int i = 0; i < tailX.Count; i++)
        {
            int tempX = tailX[i];
            int tempY = tailY[i];
            tailX[i] = prevX;
            tailY[i] = prevY;
            prevX = tempX;
            prevY = tempY;
        }

        switch (direction)
        {
            case Direction.Up:
                snakeY--;
                break;
            case Direction.Down:
                snakeY++;
                break;
            case Direction.Left:
                snakeX--;
                break;
            case Direction.Right:
                snakeX++;
                break;
        }
    }

    private void EatFood()
    {
        score++;
        tailX.Add(0);
        tailY.Add(0);
    }

    private void GenerateFood()
    {
        foodX = new Random().Next(1, width - 1);
        foodY = new Random().Next(1, height - 1);
    }

    private bool CheckCollisionWithTail()
    {
        for (int i = 0; i < tailX.Count; i++)
        {
            if (snakeX == tailX[i] && snakeY == tailY[i])
                return true;
        }
        return false;
    }

    private void DrawGame()
    {
        Console.SetCursorPosition(foodX, foodY);
        Console.Write("■");

        Console.SetCursorPosition(snakeX, snakeY);
        Console.Write("■");

        for (int i = 0; i < tailX.Count; i++)
        {
            Console.SetCursorPosition(tailX[i], tailY[i]);
            Console.Write("■");
        }

        Console.SetCursorPosition(0, height);
        Console.Write($"Score: {score}");
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        ConsoleKeyInfo keyInfo;
        int selectedItem = 0;
        bool isPlaying = false;

        Console.CursorVisible = false;

        while (true)
        {
            Console.Clear();

            if (isPlaying)
            {
                SnakeGame snakeGame = new SnakeGame(Console.WindowWidth, Console.WindowHeight - 1);
                snakeGame.PlayGame();
                isPlaying = false;
            }
            else
            {
                string[] menuItems = { "Продолжить игру", "Новая игра", "Таблица рекордов", "Выход" };

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
}

class SnakeGame
{
    private int screenWidth;
    private int screenHeight;
    private int snakeX;
    private int snakeY;
    private int foodX;
    private int foodY;
    private int score;
    private List<int> tailX;
    private List<int> tailY;
    private bool isGameOver;
    private Direction direction;

    public SnakeGame(int width, int height)
    {
        screenWidth = width;
        screenHeight = height;
        snakeX = width / 2;
        snakeY = height / 2;
        foodX = new Random().Next(1, width - 2);
        foodY = new Random().Next(1, height - 2);
        score = 0;
        tailX = new List<int>();
        tailY = new List<int>();
        isGameOver = false;
        direction = Direction.Right;
    }

    public void PlayGame()
    {
        ConsoleKeyInfo keyInfo;
        while (!isGameOver)
        {
            if (Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true);
                ChangeDirection(keyInfo.Key);
            }

            MoveSnake();
            if (snakeX == foodX && snakeY == foodY)
            {
                EatFood();
                foodX = new Random().Next(1, screenWidth - 2);
                foodY = new Random().Next(1, screenHeight - 2);
            }

            if (snakeX == 0 || snakeX == screenWidth - 1 || snakeY == 0 || snakeY == screenHeight - 1 || CheckCollisionWithTail())
            {
                isGameOver = true;
            }

            DrawGameArea();
            DrawSnake();
            DrawFood();
            Thread.Sleep(100);
        }
    }

    private void ChangeDirection(ConsoleKey key)
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
        if (tailX.Count > 0)
        {
            prevX = tailX.Last();
            prevY = tailY.Last();
            tailX.Insert(0, prevX);
            tailY.Insert(0, prevY);
            tailX.RemoveAt(tailX.Count - 1);
            tailY.RemoveAt(tailY.Count - 1);
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

    private bool CheckCollisionWithTail()
    {
        for (int i = 0; i < tailX.Count; i++)
        {
            if (snakeX == tailX[i] && snakeY == tailY[i])
                return true;
        }
        return false;
    }

    private void EatFood()
    {
        score++;
        tailX.Add(tailX.Last());
        tailY.Add(tailY.Last());
    }

    private void DrawGameArea()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Red;

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

    private void DrawSnake()
    {
        Console.SetCursorPosition(snakeX, snakeY);
        Console.Write("■");
        for (int i = 0; i < tailX.Count; i++)
        {
            Console.SetCursorPosition(tailX[i], tailY[i]);
            Console.Write("■");
        }
    }

    private void DrawFood()
    {
        Console.SetCursorPosition(foodX, foodY);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("■");
        Console.ForegroundColor = ConsoleColor.Red;
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}

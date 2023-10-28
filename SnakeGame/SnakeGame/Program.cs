﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Console;

namespace SnakeGame
{
    class Program
    {
        private const int MAP_WIDTH = 30;
        private const int MAP_HEIGHT = 20;
        private const int MAX_RECORDS = 10;
        private const string RECORDS_FILE_NAME = "records.txt";
        private const string FILE_NAME = "gameState.json";

        private const int SCREEN_WIDTH = MAP_WIDTH * 3;
        private const int SCREEN_HEIGHT = MAP_HEIGHT * 3;

        private const int FRAME_MILLISECONDS = 200;

        private const ConsoleColor BORDER_COLOR = ConsoleColor.White;
        private const ConsoleColor FOOD_COLOR = ConsoleColor.Green;
        private const ConsoleColor BODY_COLOR = ConsoleColor.White;
        private const ConsoleColor HEAD_COLOR = ConsoleColor.DarkGray;

        private static readonly Random Random = new Random();

        private static GameState gameState = GameState.MainMenu;



        static GameStateData gameStateData = new GameStateData();



        static async Task Main()
        {

            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {
                Task inputTask = HandleInputAsync(cancellationTokenSource.Token);

                GameStateData match = new GameStateData();

                bool exitRequested = false;

                LoadData();

                SetWindowSize(SCREEN_WIDTH, SCREEN_HEIGHT + 5);
                SetBufferSize(SCREEN_WIDTH, SCREEN_HEIGHT + 5);
                CursorVisible = false;
                
                List<string> records = ReadRecords(RECORDS_FILE_NAME);
                records.Sort((a, b) => int.Parse(b.Split(' ')[1]) - int.Parse(a.Split(' ')[1]));

                ConsoleKeyInfo keyInfo;
                int selectedItem = 0;

                while (!exitRequested)
                {
                    switch (gameState)
                    {
                        case GameState.MainMenu:
                            Clear();
                            Write("                                   ▄▄\n");
                            Write("███▀▀▀███                          ██ ▀███\n");
                            Write("█▀   ███                                ██\n");
                            Write("▀   ███ ▀████████▄█████▄   ▄▄█▀██▀███   ██  ▄██▀ ▄█▀██▄\n");
                            Write("   ███    ██    ██    ██  ▄█▀   ██ ██   ██ ▄█   ██   ██\n");
                            Write("  ███   ▄ ██    ██    ██  ██▀▀▀▀▀▀ ██   ██▄██    ▄█████\n");
                            Write(" ███   ▄█ ██    ██    ██  ██▄    ▄ ██   ██ ▀██▄ ██   ██\n");
                            Write("█████████████  ████  ████▄ ▀█████▀████▄████▄ ██▄▄████▀██▄\n");
                            Write("\n");
                            string[] menuItems = new string[] { "Продолжить игру", "Новая игра", "Таблица рекордов", "Выход" };

                            if (gameStateData.Snake == null)
                            {
                                // Если сохраненной игры нет, "Продолжить игру" будет неактивным (серого цвета)
                                menuItems[0] = "[Недоступно] Продолжить игру";
                            }

                            for (int i = 0; i < menuItems.Length; i++)
                            {
                                if (i == selectedItem)
                                {
                                    ForegroundColor = ConsoleColor.White;
                                }
                                else
                                {
                                    ForegroundColor = i == 0 && gameStateData.Snake == null ? ConsoleColor.Gray : ConsoleColor.White;
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
                                if (selectedItem == menuItems.Length - 1)
                                {
                                    exitRequested = true; // Устанавливаем флаг выхода
                                    cancellationTokenSource.Cancel(); // Отменяем ввод
                                }
                                else if (selectedItem == 0 && gameStateData.Snake != null)
                                {
                                    // Обработка продолжения игры
                                    gameState = GameState.InGame;
                                }
                                else if (selectedItem == 1)
                                {
                                    // Обработка начала новой игры
                                    gameState = GameState.InGame;
                                    gameStateData = new GameStateData();
                                    string gameStateJson = JsonConvert.SerializeObject(gameStateData);
                                    File.WriteAllText(FILE_NAME, gameStateJson);
                                }
                                else if (selectedItem == 2)
                                {
                                    ShowRecords(records);
                                    selectedItem = 0;
                                }
                            }
                            // Отображение главного меню
                            // Обработка клавиш для выбора опций главного меню
                            break;
                        case GameState.InGame:
                            Clear();
                            
                            match = StartGame();
                            
                            // Запуск игры
                            // Обработка ввода клавиш для управления змейкой
                            break;
                        case GameState.GameOver:
                            Clear();

                            records.Add($"{match.PlayerName} {match.Score}");
                            records.Sort((a, b) => int.Parse(b.Split(' ')[1]) - int.Parse(a.Split(' ')[1]));

                            if (records.Count > MAX_RECORDS)
                            {
                                records.RemoveAt(records.Count - 1);
                            }

                            WriteRecords(RECORDS_FILE_NAME, records);

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
                            WriteLine($"Your score: {match.Score}");

                            SetCursorPosition(1, gameOverText.Length + 4);
                            WriteLine("Press Enter to return to the main menu.");
                            while (true)
                            {
                                var keyInafo = ReadKey(true);
                                if (keyInafo.Key == ConsoleKey.Enter)
                                    gameState = GameState.MainMenu;
                                    break;
                            }
                            // Отображение экрана смерти
                            // Обработка клавиш для перезапуска игры или возврата в главное меню
                            break;
                        case GameState.Paused:
                            Clear();
                            Write("                                             \n");
                            Write("▀███▀▀▀██▄                                   \n");
                            Write("  ██   ▀██▄                                  \n");
                            Write("  ██   ▄██ ▄█▀██▄ ▀███  ▀███  ▄██▀███ ▄▄█▀██\n");
                            Write("  ███████ ██   ██   ██    ██  ██   ▀▀▄█▀   ██\n");
                            Write("  ██       ▄█████   ██    ██  ▀█████▄██▀▀▀▀▀▀\n");
                            Write("  ██      ██   ██   ██    ██  █▄   ████▄    ▄\n");
                            Write("▄████▄    ▀████▀██▄ ▀████▀███▄██████▀ ▀█████▀\n");
                            Write("                                             \n");

                            string[] menuPItems = { "Продолжить игру", "Сохранить игру", "Выйти в меню" };

                            for (int i = 0; i < menuPItems.Length; i++)
                            {
                                if (i == selectedItem)
                                {
                                    ForegroundColor = ConsoleColor.White;
                                }
                                else
                                {
                                    ForegroundColor = ConsoleColor.Gray;
                                }

                                WriteLine((i == selectedItem ? ">> " : "   ") + menuPItems[i]);
                            }

                            keyInfo = ReadKey(true);

                            if (keyInfo.Key == ConsoleKey.W && selectedItem > 0)
                            {
                                selectedItem--;
                            }
                            else if (keyInfo.Key == ConsoleKey.S && selectedItem < menuPItems.Length - 1)
                            {
                                selectedItem++;
                            }
                            else if (keyInfo.Key == ConsoleKey.Enter)
                            {
                                if (selectedItem == 2)
                                {
                                    gameState = GameState.MainMenu;
                                }
                                else if (selectedItem == 0)
                                {
                                    gameState = GameState.InGame;
                                }
                                else if (selectedItem == 1)
                                {
                                    string gameStateJson = JsonConvert.SerializeObject(gameStateData);
                                    File.WriteAllText(FILE_NAME, gameStateJson);
                                    Write("Save complete");
                                }
                            }
                            break;
                    }

                    Clear();
                    await Task.Delay(1); 
                }
                await inputTask;
            }
            
        }

        private static void LoadData()
        {
            if (File.Exists(FILE_NAME))
            {
                string json = File.ReadAllText(FILE_NAME); // Чтение JSON из файла
                gameStateData = JsonConvert.DeserializeObject<GameStateData>(json);
            }
            else
            {
                // Обработка ситуации, когда файл не существует
                gameStateData = new GameStateData();
            }
        }

        enum GameState
        {
            MainMenu,
            InGame,
            GameOver,
            Paused
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
                        pauseRequested = true;
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

        static bool pauseRequested = false;

        static GameStateData StartGame()
        {

            string playerName = "";
            bool isGameOver = false;
            
            Snake snake;
            Pixel food;
            int score = 0;
            GameStateData matchData = new GameStateData();


            if (gameStateData.Snake == null)
            {

                playerName = GetPlayerName();

                snake = new Snake(10, 5, HEAD_COLOR, BODY_COLOR);
                food = GenFood(snake);
            }
            else
            {
                playerName = gameStateData.PlayerName;
                snake = gameStateData.Snake;
                food = gameStateData.Food;
                score = gameStateData.Score;
            }
            Clear();
            DrawBoard();
            food.Draw();

            Direction currentMovement = Direction.Right;


            int lagMs = 0;
            var sw = new Stopwatch();

            string infoText = $"| Текущий счёт: {score} |";
            int x = (SCREEN_WIDTH - infoText.Length) / 2;
            SetCursorPosition((SCREEN_WIDTH - playerName.Length)/2, SCREEN_HEIGHT + 1);
            Write($"{playerName}");
            SetCursorPosition((SCREEN_WIDTH - 15) / 2, SCREEN_HEIGHT + 3);
            Write($"| ESC - Пауза |");
            while (!isGameOver && !pauseRequested)
            {
                SetCursorPosition(0, 0);

                // Очистка предыдущей строки
                Write(new string(' ', SCREEN_WIDTH));
                SetCursorPosition(x, 1);
                // Вывод строки с информацией
                Write($"| Текущий счёт: {score} |");

                sw.Restart();
                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FRAME_MILLISECONDS - lagMs)
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

                if (snake.Head.X == MAP_WIDTH - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MAP_HEIGHT - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                {
                    isGameOver = true;
                }

                lagMs = (int)sw.ElapsedMilliseconds;
            }

            matchData.Snake = snake;
            matchData.Food = food;
            matchData.Score = score;
            matchData.PlayerName = playerName;

            if (pauseRequested)
            {
                pauseRequested = false;
                gameStateData = matchData;
                gameState = GameState.Paused;
            }
            else
            {
                gameStateData = new GameStateData();
                for (int i = 1; i < MAP_WIDTH - 1; i++)
                {
                    for (int j = 1; j < MAP_HEIGHT - 1; j++)
                    {
                        SetCursorPosition(i * 3, j);
                        Write(" ");
                    }
                }
                gameState = GameState.GameOver;
            }

            snake.Clear();
            food.Clear();
            return matchData;
        }


        static string GetPlayerName()
        {
            Clear();
            Write("Введите ваш ник: ");
            return ReadLine();
        }

        static void DrawBoard()
        {
            for (int i = 0; i < MAP_WIDTH; i++)
            {
                new Pixel(i, 0, BORDER_COLOR).Draw();
                new Pixel(i, MAP_HEIGHT - 1, BORDER_COLOR).Draw();
            }

            for (int i = 0; i < MAP_HEIGHT; i++)
            {
                new Pixel(0, i, BORDER_COLOR).Draw();
                new Pixel(MAP_WIDTH - 1, i, BORDER_COLOR).Draw();
            }
        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;

            do
            {
                food = new Pixel(Random.Next(1, MAP_WIDTH - 2), Random.Next(1, MAP_HEIGHT - 2), FOOD_COLOR);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y ||
                     snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }


        static List<string> ReadRecords(string FILE_NAME)
        {
            List<string> records = new List<string>();

            if (File.Exists(FILE_NAME))
            {
                records = File.ReadAllLines(FILE_NAME).ToList();
            }

            return records;
        }

        static void WriteRecords(string FILE_NAME, List<string> records)
        {
            File.WriteAllLines(FILE_NAME, records);
        }

        static void ShowRecords(List<string> records)
        {
            Clear();

            WriteLine("Таблица рекордов:");

            for (int i = 0; i < records.Count; i++)
            {
                if (i >= MAX_RECORDS)
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


    [Serializable]
    public class GameStateData
    {
        public Snake Snake { get; set; }
        public Pixel Food { get; set; }
        public int Score { get; set; }
        public string PlayerName { get; set; }

        public GameStateData()
        {
            // Инициализация всех свойств, которые нужно сохранить
        }
        // Другие параметры, которые вы хотите сохранить
    }

}

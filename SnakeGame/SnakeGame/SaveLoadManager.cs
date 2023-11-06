using System;
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
    internal class SaveLoadManager
    {

        private const string RECORDS_FILE_NAME = "records.txt";
        private const string FILE_NAME = "gameState.json";

        /// <summary>
        /// Загружает данные игры из файла, если такие данные существуют.
        /// </summary>
        public GameStateData LoadData()
        {
            GameStateData data;
            if (File.Exists(FILE_NAME))
            {
                string json = File.ReadAllText(FILE_NAME); // Чтение JSON из файла
                data = JsonConvert.DeserializeObject<GameStateData>(json) ?? new GameStateData();
            }
            else
            {
                // Обработка ситуации, когда файл не существует.
                data = new GameStateData();
            }
            return data;
        }
        public void SaveData(GameStateData gameStateData)
        {
            string gameStateJson = JsonConvert.SerializeObject(gameStateData);
            File.WriteAllText(FILE_NAME, gameStateJson);
            Console.Write("Сохранение произошло");
            System.Threading.Thread.Sleep(1000);
            // Очистить фразу после ожидания
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', "Сохранение произошло".Length));
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        public void QuietSave(GameStateData gameStateData)
        {
            string gameStateJson = JsonConvert.SerializeObject(gameStateData);
            File.WriteAllText(FILE_NAME, gameStateJson);
        }

        /// <summary>
        /// Считывает записи рекордов из файла.
        /// </summary>
        /// <param name="FILE_NAME"></param>
        /// <returns></returns>
        public List<string> ReadRecords()
        {
            List<string> records = new List<string>();

            if (File.Exists(RECORDS_FILE_NAME))
            {
                records = File.ReadAllLines(RECORDS_FILE_NAME).ToList();
            }

            return records;
        }

        /// <summary>
        /// Записывает записи рекордов в файл.
        /// </summary>
        /// <param name="FILE_NAME"></param>
        /// <param name="records"></param>
        public void WriteRecords(List<string> records)
        {
            File.WriteAllLines(RECORDS_FILE_NAME, records);
        }
    }
}

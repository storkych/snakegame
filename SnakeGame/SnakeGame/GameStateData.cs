using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    /// <summary>
    /// Класс, представляющий данные о текущем состоянии игры.
    /// </summary>
    [Serializable]
    public class GameStateData
    {
        public bool IsSavedGame = false;

        //
        public Pixel Food { get; set; }

        //
        public int Score { get; set; }

        //
        public string PlayerName { get; set; }

        //
        public Pixel Head { get; set; }

        //
        public Queue<Pixel> Body { get; set; }

        //
        public Direction SnakeDir { get; set; }

    }
}

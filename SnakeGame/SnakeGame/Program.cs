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
    class Program
    {

        /// <summary>
        /// Главный метод, который запускает игру.
        /// </summary>
        /// <returns></returns>
        static void Main()
        {
            GameEngine gameEngine = new GameEngine();
            gameEngine.Run();
        }

    }


}

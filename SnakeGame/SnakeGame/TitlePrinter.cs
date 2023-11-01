using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using static System.Console;

namespace SnakeGame
{
    /// <summary>
    /// 
    /// </summary>
    internal class TitlePrinter
    {
        /// <summary>
        /// 
        /// </summary>
        public void TitlePrint()
        {
            Write("                                   ▄▄\n");
            Write("███▀▀▀███                          ██ ▀███\n");
            Write("█▀   ███                                ██\n");
            Write("▀   ███ ▀████████▄█████▄   ▄▄█▀██▀███   ██  ▄██▀ ▄█▀██▄\n");
            Write("   ███    ██    ██    ██  ▄█▀   ██ ██   ██ ▄█   ██   ██\n");
            Write("  ███   ▄ ██    ██    ██  ██▀▀▀▀▀▀ ██   ██▄██    ▄█████\n");
            Write(" ███   ▄█ ██    ██    ██  ██▄    ▄ ██   ██ ▀██▄ ██   ██\n");
            Write("█████████████  ████  ████▄ ▀█████▀████▄████▄ ██▄▄████▀██▄\n");
            Write("\n");
        }

        /// <summary>
        /// 
        /// </summary>
        public void GameOverPrint()
        {
            Write("  ▄▄ •  ▄▄▄· • ▌ ▄ ·. ▄▄▄ .         ▌ ▐·▄▄▄ .▄▄▄  \n");
            Write(" ▐█ ▀ ▪▐█ ▀█ ·██ ▐███▪▀▄.▀·   ▄█▀▄ ▪█·█▌▀▄.▀·▀▄ █·\n");
            Write(" ▄█ ▀█▄▄█▀▀█ ▐█ ▌▐▌▐█·▐▀▀▪▄  ▐█▌.▐▌▐█▐█•▐▀▀▪▄▐▀▀▄ \n");
            Write(" ▐█▄▪▐█▐█▪ ▐▌██ ██▌▐█▌▐█▄▄▌  ▐█▌.▐▌ ███ ▐█▄▄▌▐█•█▌\n");
            Write("  ·▀▀▀▀  ▀  ▀ ▀▀  █▪▀▀▀ ▀▀▀    ▀█▄▀▪. ▀   ▀▀▀ .▀  ▀\n");
            Write("\n");
        }

        /// <summary>
        /// 
        /// </summary>
        public void PausePrint()
        {
            Write("                                             \n");
            Write("▀███▀▀▀██▄                                   \n");
            Write("  ██   ▀██▄                                  \n");
            Write("  ██   ▄██ ▄█▀██▄ ▀███  ▀███  ▄██▀███ ▄▄█▀██\n");
            Write("  ███████ ██   ██   ██    ██  ██   ▀▀▄█▀   ██\n");
            Write("  ██       ▄█████   ██    ██  ▀█████▄██▀▀▀▀▀▀\n");
            Write("  ██      ██   ██   ██    ██  █▄   ████▄    ▄\n");
            Write("▄████▄    ▀████▀██▄ ▀████▀███▄██████▀ ▀█████▀\n");
            Write("                                             \n");
            Write("\n");
        }
    }
}

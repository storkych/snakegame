using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    internal class TitlePrinter
    {
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

        public void GameOverPrint()
        {
            Write("  ▄▄ •  ▄▄▄· • ▌ ▄ ·. ▄▄▄ .         ▌ ▐·▄▄▄ .▄▄▄  \n");
            Write(" ▐█ ▀ ▪▐█ ▀█ ·██ ▐███▪▀▄.▀·   ▄█▀▄ ▪█·█▌▀▄.▀·▀▄ █·\n");
            Write(" ▄█ ▀█▄▄█▀▀█ ▐█ ▌▐▌▐█·▐▀▀▪▄  ▐█▌.▐▌▐█▐█•▐▀▀▪▄▐▀▀▄ \n");
            Write(" ▐█▄▪▐█▐█▪ ▐▌██ ██▌▐█▌▐█▄▄▌  ▐█▌.▐▌ ███ ▐█▄▄▌▐█•█▌\n");
            Write("  ·▀▀▀▀  ▀  ▀ ▀▀  █▪▀▀▀ ▀▀▀    ▀█▄▀▪. ▀   ▀▀▀ .▀  ▀\n");
            Write("\n");
        }

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

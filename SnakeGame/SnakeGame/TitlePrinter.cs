﻿using static System.Console;

namespace SnakeGame
{
    /// <summary>
    /// Класс, отвечающий за отрисовку заголовков.
    /// </summary>
    internal class TitlePrinter
    {
        /// <summary>
        /// Отрисовка заголовка игры над основным меню.
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
        /// Отрисовка экрана смерти в результате смерти.
        /// </summary>
        public void GameOverPrint()
        {
            Write("  ▄▄    ▄▄▄    ▌ ▄    ▄▄▄           ▌ ▐ ▄▄▄  ▄▄▄  \n");
            Write(" ▐█ ▀  ▐█ ▀█  ██ ▐███ ▀▄ ▀    ▄█▀▄  █ █▌▀▄ ▀ ▀▄ █ \n");
            Write(" ▄█ ▀█▄▄█▀▀█ ▐█ ▌▐▌▐█ ▐▀▀ ▄  ▐█▌ ▐▌▐█▐█ ▐▀▀ ▄▐▀▀▄ \n");
            Write(" ▐█▄ ▐█▐█  ▐▌██ ██▌▐█▌▐█▄▄▌  ▐█▌ ▐▌ ███ ▐█▄▄▌▐█ █▌\n");
            Write("  ▀▀▀▀  ▀  ▀ ▀▀  █ ▀▀▀ ▀▀▀    ▀█▄▀   ▀   ▀▀▀  ▀  ▀\n");
            Write("\n");
        }

        /// <summary>
        /// Отрисовка экрана паузы над меню паузы.
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

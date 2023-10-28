using Newtonsoft.Json;
using System;
using static System.Console;

namespace SnakeGame
{
    public readonly struct Pixel
    {
        private const char PIXEL_CHAR = '█';

        public Pixel(int x, int y, ConsoleColor color, int pixelSize = 3)
        {
            X = x;
            Y = y;
            Color = color;
            PixelSize = pixelSize;
        }
        [JsonProperty]
        public int X { get; }
        [JsonProperty]
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
                    Write(PIXEL_CHAR);
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
}

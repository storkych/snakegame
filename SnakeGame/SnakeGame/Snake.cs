using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SnakeGame
{
    public class Snake
    {
        private readonly ConsoleColor _headColor;
        private readonly ConsoleColor _bodyColor;

        public Snake() {
            _headColor = ConsoleColor.DarkGray;
            _bodyColor = ConsoleColor.White;

            Head = new Pixel(5, 5, _headColor);

            for (var i = 3; i >= 0; i--)
            {
                Body.Enqueue(new Pixel(Head.X - i - 1, 5, _bodyColor));
            }
        }

        public Snake(Pixel head, Queue<Pixel> body)
        {
            _headColor = ConsoleColor.DarkGray;
            _bodyColor = ConsoleColor.White;
            Head = head;
            Body = body;
        }

        public Snake(int initialX,
            int initialY,
            ConsoleColor headColor,
            ConsoleColor bodyColor,
            int bodyLength = 3)
        {
            _headColor = headColor;
            _bodyColor = bodyColor;

            Head = new Pixel(initialX, initialY, headColor);

            for (var i = bodyLength; i >= 0; i--)
            {
                Body.Enqueue(new Pixel(Head.X - i - 1, initialY, _bodyColor));
            }

            Draw();
        }
        [JsonProperty]
        public Pixel Head { get; private set; }
        [JsonProperty]
        public Queue<Pixel> Body { get; } = new Queue<Pixel>();

        /// <summary>
        /// Перемещает змейку в заданном направлении и обновляет ее позицию.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="eat"></param>
        public void Move(Direction direction, bool eat = false)
        {
            Clear();

            Body.Enqueue(new Pixel(Head.X, Head.Y, _bodyColor));
            if (!eat)
                Body.Dequeue();

            if (direction == Direction.Right)
            {
                Head = new Pixel(Head.X + 1, Head.Y, _headColor);
            }
            else if (direction == Direction.Left)
            {
                Head = new Pixel(Head.X - 1, Head.Y, _headColor);
            }
            else if (direction == Direction.Up)
            {
                Head = new Pixel(Head.X, Head.Y - 1, _headColor);
            }
            else if (direction == Direction.Down)
            {
                Head = new Pixel(Head.X, Head.Y + 1, _headColor);
            }

            Draw();
        }
        /// <summary>
        /// Отрисовывает змейку на экране.
        /// </summary>
        public void Draw()
        {
            Head.Draw();

            foreach (Pixel pixel in Body)
            {
                pixel.Draw();
            }
        }
        /// <summary>
        /// Убирает змейку с экрана.
        /// </summary>
        public void Clear()
        {
            Head.Clear();

            foreach (Pixel pixel in Body)
            {
                pixel.Clear();
            }
        }
    }
}

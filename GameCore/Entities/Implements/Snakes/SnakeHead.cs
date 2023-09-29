using GameCore.Entities.Interfaces;
using System;
using System.Drawing;

namespace GameCore.Entities.Implements.Games
{
    public class SnakeHead : ISnakeBody
    {
        public ISnake Snake { private set; get; }
        public Point Position { private set; get; }
        public event EventHandler<EventArgs> OnDisposed;

        public int BorderWidth => this.Snake.BorderWidth;
        public Pen Border => this.Snake.Border;
        public Brush Color => Brushes.Red;

        public SnakeHead(ISnake Snake, int X, int Y)
        {
            this.Snake = Snake;
            this.Position = new Point(X, Y);
        }

        public void MoveTo(Point Position)
        {
            this.Position = Position;
        }

        public void Dispose()
        {
            this.OnDisposed?.Invoke(this, EventArgs.Empty);
        }
    }
}

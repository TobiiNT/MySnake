using GameCore.Entities.Interfaces.Snakes;
using System;
using System.Drawing;

namespace GameCore.Entities.Implements.Snakes
{
    public class SnakeBody : ISnakeBody
    {
        public ISnake Snake { private set; get; }
        public Point Position { private set; get; }
        public event EventHandler<EventArgs> OnDisposed;

        public int BorderWidth => this.Snake.BorderWidth;
        public Pen Border => this.Snake.Border;
        public Brush Color => this.Snake.Color;

        public SnakeBody(ISnake Snake, int X, int Y)
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

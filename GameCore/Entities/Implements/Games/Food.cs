using GameCore.Entities.Interfaces.Games;
using GameCore.Entities.Interfaces.Snakes;
using System;
using System.Drawing;

namespace GameCore.Entities.Implements.Games
{
    public class Food : IFood
    {
        public Point Position { private set; get; }
        public event EventHandler<EventArgs> OnDisposed;

        public float Size => 0.5f;
        public int BorderWidth => 7;
        public Color BorderColor => Color.Blue; 
        public Color FillColor => Color.Orange;

        public Food(int X, int Y)
        {
            this.Position = new Point(X, Y);
        }

        public void ApplyEffect(ISnake Snake)
        {
            Snake.AddLength(1);
        }

        public void Dispose()
        {
            this.OnDisposed?.Invoke(this, EventArgs.Empty);
        }
    }
}

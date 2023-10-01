using GameCore.Entities.Interfaces.Games;
using System;
using System.Drawing;

namespace GameCore.Entities.Implements.Games
{
    public class Obstacle : IGameObject
    {
        public Point Position { private set; get; }
        public event EventHandler<EventArgs> OnDisposed;

        public float Size => 1f;
        public int BorderWidth => 1;
        public Color BorderColor => Color.LightCyan; 
        public Color FillColor => Color.MediumSeaGreen;


        public Obstacle(int X, int Y)
        {
            this.Position = new Point(X, Y);
        }


        public void Dispose()
        {
            this.OnDisposed?.Invoke(this, EventArgs.Empty);
        }
    }
}

using GameCore.Entities.Interfaces;
using System;
using System.Drawing;

namespace GameCore.Entities.Implements.Games
{
    public class Obstacle : IGameObject
    {
        public Point Position { private set; get; }
        public event EventHandler<EventArgs> OnDisposed;

        public int BorderWidth => 1;
        public Pen Border => Pens.LightCyan; 
        public Brush Color => Brushes.MediumSeaGreen;

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

using GameCore.Entities.Interfaces;
using System;
using System.Drawing;

namespace GameCore.Entities.Implements.Games
{
    public class Food : IGameObject
    {
        public Point Position { private set; get; }
        public event EventHandler<EventArgs> OnDisposed;

        public int BorderWidth => 7;
        public Pen Border => Pens.Blue; 
        public Brush Color => Brushes.Orange;


        public Food(int X, int Y)
        {
            this.Position = new Point(X, Y);
        }

        public void Dispose()
        {
            this.OnDisposed?.Invoke(this, EventArgs.Empty);
        }
    }
}

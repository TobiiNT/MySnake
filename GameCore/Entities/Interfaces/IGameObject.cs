using System;
using System.Drawing;

namespace GameCore.Entities.Interfaces
{
    public interface IGameObject : IDrawable, IDisposable
    {
        Point Position { get; }
        event EventHandler<EventArgs> OnDisposed;
    }
}

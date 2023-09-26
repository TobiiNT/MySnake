using System.Drawing;

namespace GameCore.Entities.Interfaces
{
    public interface IDrawable
    {
        int BorderWidth { get; }
        Pen Border { get; }
        Brush Color { get; }
    }
}

using System.Drawing;

namespace GameCore.Entities.Interfaces.Games
{
    public interface IDrawable
    {
        int BorderWidth { get; }
        Pen Border { get; }
        Brush Color { get; }
    }
}

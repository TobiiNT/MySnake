using System.Drawing;

namespace GameCore.Entities.Interfaces.Games
{
    public interface IDrawable
    {
        float Size { get; }
        int BorderWidth { get; }
        Color BorderColor { get; }
        Color FillColor { get; }
    }
}

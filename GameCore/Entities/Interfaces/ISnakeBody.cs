using System.Drawing;

namespace GameCore.Entities.Interfaces
{
    public interface ISnakeBody : IGameObject
    {
        ISnake Snake { get; }
        void MoveTo(Point Position);
    }
}

using GameCore.Entities.Interfaces.Games;
using System.Drawing;

namespace GameCore.Entities.Interfaces.Snakes
{
    public interface ISnakeBody : IGameObject
    {
        ISnake Snake { get; }
        void MoveTo(Point Position);
    }
}

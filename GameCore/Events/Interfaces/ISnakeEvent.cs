using GameCore.Entities.Interfaces.Snakes;

namespace GameCore.Events.Interfaces
{
    public interface ISnakeEvent
    {
        ISnake Snake { get; }
    }
}

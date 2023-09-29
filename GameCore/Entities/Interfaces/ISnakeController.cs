using GameCore.Entities.Enums;
using GameCore.Entities.Implements.Snakes;

namespace GameCore.Entities.Interfaces
{
    public interface ISnakeController
    {
        Direction GetNextMove(Snake Snake);
    }
}

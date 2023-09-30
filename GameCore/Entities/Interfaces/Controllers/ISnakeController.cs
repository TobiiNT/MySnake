using GameCore.Entities.Enums;
using GameCore.Entities.Implements.Snakes;

namespace GameCore.Entities.Interfaces.Controllers
{
    public interface ISnakeController
    {
        Direction GetNextMove(Snake Snake);
    }
}

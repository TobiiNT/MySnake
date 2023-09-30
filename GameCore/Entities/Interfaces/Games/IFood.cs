using GameCore.Entities.Interfaces.Snakes;

namespace GameCore.Entities.Interfaces.Games
{
    public interface IFood : IGameObject
    {
        void ApplyEffect(ISnake Snake);
    }
}

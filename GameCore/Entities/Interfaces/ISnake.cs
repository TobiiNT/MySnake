using GameCore.Entities.Enums;
using System.Collections.Generic;

namespace GameCore.Entities.Interfaces
{
    public interface ISnake : IGameObject
    {
        List<ISnakeBody> Bodies { get; }

        SnakeState State { get; }
        bool IsMoving { get; }
        int MoveSpeed { get; }
        Direction Direction { get; }
        
        void ChangeState(SnakeState State);
        void ChangeSpeed(int Speed);
        void ChangeDirection(Direction Direction);
        void Move();
        void Die();

        int PendingBodies { get; }
        void AddLength(int Length);
    }
}

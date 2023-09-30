using GameCore.Entities.Enums;
using GameCore.Entities.Interfaces.Controllers;
using GameCore.Entities.Interfaces.Games;
using System;
using System.Collections.Generic;

namespace GameCore.Entities.Interfaces.Snakes
{
    public interface ISnake : IGameObject
    {
        ISnakeController Controller { get; }
        void SetController(ISnakeController Controller);

        ISnakeBody Head { get; }
        ISnakeBody Tail { get; }
        List<ISnakeBody> Bodies { get; }
        int Length { get; }
        SnakeState State { get; }
        bool IsMoving { get; }
        int MoveSpeed { get; }
        Direction Direction { get; }
        DateTime LastMoveTime { get; }

        void ChangeState(SnakeState State);
        void ChangeSpeed(int Speed);
        void ChangeDirection(Direction Direction);
        void Move();
        void Die();

        int PendingBodies { get; }
        void AddLength(int Length);
    
        event EventHandler<EventArgs> OnSnakeMoving;
        event EventHandler<EventArgs> OnSnakeLengthChanged; 
    }
}

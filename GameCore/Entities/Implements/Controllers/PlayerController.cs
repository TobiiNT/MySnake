using GameCore.Entities.Enums;
using GameCore.Entities.Implements.Snakes;
using GameCore.Entities.Interfaces.Controllers;
using System;

namespace GameCore.Entities.Implements.Controllers
{
    public class PlayerController : ISnakeController
    {
        private Direction PlayerInputDirection { set; get; }

        public event Action<Direction> OnDirectionInput;

        public Direction GetNextMove(Snake Snake) => PlayerInputDirection;

        public void SetDirection(Direction Direction)
        {
            this.PlayerInputDirection = Direction;
            OnDirectionInput?.Invoke(Direction);
        }
    }
}

using GameCore.Entities.Enums;
using GameCore.Entities.Interfaces.Snakes;
using System;
using System.Drawing;

namespace GameCore.Events
{
    public class OnSnakeDirectionChanged : EventArgs
    {
        public ISnake Snake { private set; get; }
        public Direction NewDirection { private set; get; }
        public OnSnakeDirectionChanged(ISnake Snake, Direction NewDirection)
        {
            this.Snake = Snake;
            this.NewDirection = NewDirection;
        }
    }
}

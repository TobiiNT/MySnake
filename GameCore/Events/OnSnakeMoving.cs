using GameCore.Entities.Interfaces;
using System;
using System.Drawing;

namespace GameCore.Events
{
    public class OnSnakeMoving : EventArgs
    {
        public ISnake Snake { private set; get; }
        public Point LastTailPosition { private set; get; }
        public OnSnakeMoving(ISnake Snake, Point LastTailPosition)
        {
            this.Snake = Snake;
            this.LastTailPosition = LastTailPosition;
        }
    }
}

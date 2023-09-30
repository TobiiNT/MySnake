using GameCore.Entities.Interfaces.Snakes;
using GameCore.Events.Interfaces;
using System;

namespace GameCore.Events
{
    public class OnSnakeLengthChanged : EventArgs, ISnakeEvent
    {
        public ISnake Snake { private set; get; }
        public int NewLength { private set; get; }
        public OnSnakeLengthChanged(ISnake Snake, int NewLength)
        {
            this.Snake = Snake;
            this.NewLength = NewLength;
        }
    }
}

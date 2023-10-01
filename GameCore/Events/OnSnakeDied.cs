using GameCore.Entities.Interfaces.Snakes;
using System;
using System.Drawing;

namespace GameCore.Events
{
    public class OnSnakeDied : EventArgs
    {
        public ISnake Snake { private set; get; }
        public OnSnakeDied(ISnake Snake)
        {
            this.Snake = Snake;
        }
    }
}

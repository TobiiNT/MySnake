﻿using GameCore.Entities.Interfaces;
using GameCore.Events.Interfaces;
using System;
using System.Drawing;

namespace GameCore.Events
{
    public class OnSnakeLengthChanged : EventArgs, ISnakeEvent
    {
        public ISnake Snake { private set; get; }
        public Point LastTailPosition { private set; get; }
        public OnSnakeLengthChanged(ISnake Snake, Point LastTailPosition)
        {
            this.Snake = Snake;
            this.LastTailPosition = LastTailPosition;
        }
    }
}
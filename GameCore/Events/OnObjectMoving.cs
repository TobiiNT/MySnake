using System;
using System.Drawing;

namespace GameCore.Events
{
    public class OnObjectMoving : EventArgs
    {
        public Point Point { private set; get; }
        public OnObjectMoving(Point Point)
        {
            this.Point = Point;
        }
    }
}

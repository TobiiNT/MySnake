using System.Drawing;

namespace MySnake
{
    internal class Constants
    {
        public static int Block_Size = 20;

        public static int Snake_Default_Size = 3;

        public static Pen Background_Border = Pens.White;
        public static Brush Background_Color = Brushes.White;

        public static Pen Obstacle_Border = Pens.LightCyan;
        public static Brush Obstacle_Color = Brushes.MediumSeaGreen;

        public static Pen Food_Border = Pens.Blue;
        public static Brush Food_Color = Brushes.Orange;

        public static Pen Snake_Border = Pens.BlueViolet;
        public static Brush Snake_Head = Brushes.Red;
    }
}

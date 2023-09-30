using GameCore.Entities.Interfaces.Games;
using GameCore.Entities.Interfaces.Snakes;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MySnake
{
    public class Render
    {
        private static object LockObject = new object();

        public static void Draw(Graphics Graphic, IGameObject Object)
        {
            Draw(Graphic, new Point(Object.Position.X * Constants.BlockSize, Object.Position.Y * Constants.BlockSize), Object.Border, Object.Color, Constants.BlockSize, Object.BorderWidth);
        }

        public static void DrawSnake(Graphics Graphic, ISnake Snake)
        {
            Draw(Graphic, Snake.Head);
            for (int i = 1; i < Snake.Length; i++)
                Draw(Graphic, Snake.Bodies[i]);

        }

        private static void Draw(Graphics Graphic, Point Point, Pen Pen, Brush Brush, int Width, int Radius)
        {
            try
            {
                lock (LockObject)
                {
                    GraphicsPath GraphicPath = new GraphicsPath();
                    GraphicPath.AddLine(Point.X + Radius, Point.Y, Point.X + Width - (Radius * 2), Point.Y);
                    GraphicPath.AddArc(Point.X + Width - (Radius * 2), Point.Y, Radius * 2, Radius * 2, 270, 90);
                    GraphicPath.AddLine(Point.X + Width, Point.Y + Radius, Point.X + Width, Point.Y + Width - (Radius * 2));
                    GraphicPath.AddArc(Point.X + Width - (Radius * 2), Point.Y + Width - (Radius * 2), Radius * 2, Radius * 2, 0, 90);
                    GraphicPath.AddLine(Point.X + Width - (Radius * 2), Point.Y + Width, Point.X + Radius, Point.Y + Width);
                    GraphicPath.AddArc(Point.X, Point.Y + Width - (Radius * 2), Radius * 2, Radius * 2, 90, 90);
                    GraphicPath.AddLine(Point.X, Point.Y + Width - (Radius * 2), Point.X, Point.Y + Radius);
                    GraphicPath.AddArc(Point.X, Point.Y, Radius * 2, Radius * 2, 180, 90);

                    GraphicPath.CloseFigure();
                    Graphic.FillPath(Brush, GraphicPath);
                    Graphic.DrawPath(Pen, GraphicPath);
                    GraphicPath.Dispose();
                }

            }
            catch { }
        }
    }
}

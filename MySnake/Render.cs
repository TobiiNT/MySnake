using GameCore.Entities.Interfaces;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MySnake
{
    public class Render
    {
        private static object LockObject = new object();

        public static void Draw(Graphics g, Point p, Pen pen, Brush brush, int Width = 20, int Radius = 3)
        {
            try
            {
                lock (LockObject)
                {
                    GraphicsPath gp = new GraphicsPath();
                    gp.AddLine(p.X + Radius, p.Y, p.X + Width - (Radius * 2), p.Y);
                    gp.AddArc(p.X + Width - (Radius * 2), p.Y, Radius * 2, Radius * 2, 270, 90);
                    gp.AddLine(p.X + Width, p.Y + Radius, p.X + Width, p.Y + Width - (Radius * 2));
                    gp.AddArc(p.X + Width - (Radius * 2), p.Y + Width - (Radius * 2), Radius * 2, Radius * 2, 0, 90);
                    gp.AddLine(p.X + Width - (Radius * 2), p.Y + Width, p.X + Radius, p.Y + Width);
                    gp.AddArc(p.X, p.Y + Width - (Radius * 2), Radius * 2, Radius * 2, 90, 90);
                    gp.AddLine(p.X, p.Y + Width - (Radius * 2), p.X, p.Y + Radius);
                    gp.AddArc(p.X, p.Y, Radius * 2, Radius * 2, 180, 90);

                    gp.CloseFigure();
                    g.FillPath(brush, gp);
                    g.DrawPath(pen, gp);
                    gp.Dispose();
                }

            }
            catch { }
        }

        private static int Size = 20;

        public static void Draw(Graphics Graphic, IGameObject Object)
        {
            Render.Draw(Graphic, new Point(Object.Position.X * Size, Object.Position.Y * Size), Object.Border, Object.Color, Size, Object.BorderWidth);
        }

        public static void DrawSnake(Graphics Graphic, ISnake Snake)
        {
            Render.Draw(Graphic, Snake.Head);
            for (int i = 1; i < Snake.Length; i++)
                Render.Draw(Graphic, Snake.Bodies[i]);

        }
    }
}

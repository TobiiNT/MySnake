using GameCore.Entities.Implements.Games;
using GameCore.Entities.Interfaces.Games;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MySnake
{
    internal class OpenGL
    {
        private IMatrix Matrix;
        private Color BackgroundColor;

        public OpenGL(IMatrix Matrix, Color BackgroundColor)
        {
            this.Matrix = Matrix;
            this.BackgroundColor = BackgroundColor;
        }

        public void DrawBackground()
        {
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            // Set the color for the rectangle fill
            GL.Color3(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B);

            GL.Begin(PrimitiveType.Quads); // Change to Quads
            GL.Vertex2(-1, 1); // Top left
            GL.Vertex2(1, 1); // Top right
            GL.Vertex2(1, -1); // Bottom right
            GL.Vertex2(-1, -1); // Bottom left
            GL.End();

            // Reset color to default (white)
            GL.Color3(1.0f, 1.0f, 1.0f);
        }

        public void DrawObject(IGameObject Object)
        {
            DrawRectangle(Object.Position.X, Object.Position.Y, Object.Size, Object.Size, Object.FillColor);
        }

        private void DrawRectangle(float X, float Y, float Width, float Height, Color Color)
        {
            X = X / Matrix.Width * 2 - 1;
            Y = Y / Matrix.Height * 2 - 1;
            Width /= Matrix.Width;
            Height /= Matrix.Height;
            float PaddingX = 1f / Matrix.Width - Width;
            float PaddingY = 1f / Matrix.Width - Height;

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            // Set the color for the rectangle fill
            GL.Color3(Color.R, Color.G, Color.B);

            GL.Begin(PrimitiveType.Quads); // Change to Quads
            GL.Vertex2(X + PaddingX, PaddingY + Y + Height * 2); // Top left
            GL.Vertex2(X + PaddingX + Width * 2, PaddingY + Y + Height * 2); // Top right
            GL.Vertex2(X + PaddingX + Width * 2, PaddingY + Y); // Bottom right
            GL.Vertex2(X + PaddingX, PaddingY + Y); // Bottom left
            GL.End();

            // Reset color to default (white)
            GL.Color3(1.0f, 1.0f, 1.0f);
        }

        private void DrawBorder(float X, float Y, float Width, float Height, Color Border)
        {
            X = X / Matrix.Width * 2 - 1;
            Y = Y / Matrix.Height * 2 - 1;
            Width /= Matrix.Width;
            Height /= Matrix.Height;
            float PaddingX = 1f / Matrix.Width - Width;
            float PaddingY = 1f / Matrix.Width - Height;

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            // Draw the border around the rectangle
            GL.Color3(Border.R, Border.G, Border.B);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(X + PaddingX, PaddingY + Y + Height * 2); // Top left
            GL.Vertex2(X + PaddingX + Width * 2, PaddingY + Y + Height * 2); // Top right
            GL.Vertex2(X + PaddingX + Width * 2, PaddingY + Y); // Bottom right
            GL.Vertex2(X + PaddingX, PaddingY + Y); // Bottom left
            GL.End();

            // Reset color to default (white)
            GL.Color3(1.0f, 1.0f, 1.0f);
        }
    }
}

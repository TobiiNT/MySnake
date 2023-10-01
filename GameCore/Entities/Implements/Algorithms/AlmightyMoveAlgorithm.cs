using GameCore.Entities.Enums;
using GameCore.Entities.Interfaces.Algorithms;
using GameCore.Entities.Interfaces.Games;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameCore.Entities.Implements.Algorithms
{
    public class AlmightyMoveAlgorithm : IPathAlgorithm
    {
        public IMatrix Matrix { get; }
        public List<Point> Goals { get; }

        public AlmightyMoveAlgorithm(IMatrix Matrix)
        {
            this.Matrix = Matrix;
           
            this.Goals = new List<Point>();
        }

        public List<Point> FindPath(Point StartPosition)
        {
            Direction NextMove = Direction.RIGHT; // Default move

            // Check if either width or height is even
            if (Matrix.Width % 2 == 0 || Matrix.Height % 2 == 0)
            {
                // Implement optimal circuit for even-dimensioned board
                if (StartPosition.X == 0 && StartPosition.Y < Matrix.Height - 1)
                {
                    NextMove = Direction.DOWN;
                }
                else if (StartPosition.Y == Matrix.Height - 1 && StartPosition.X < Matrix.Width - 1)
                {
                    NextMove = Direction.RIGHT;
                }
                else if (StartPosition.X == Matrix.Width - 1 && StartPosition.Y > 0)
                {
                    NextMove = Direction.UP;
                }
                else if (StartPosition.Y == 0 && StartPosition.X > 0)
                {
                    NextMove = Direction.LEFT;
                }
            }
            else
            {
                // Implement circuit for odd-dimensioned board
                // For demonstration, following a similar pattern as even-dimensioned board
                if (StartPosition.X == 0 && StartPosition.Y < Matrix.Height - 1)
                {
                    NextMove = Direction.DOWN;
                }
                else if (StartPosition.Y == Matrix.Height - 1 && StartPosition.X < Matrix.Width - 1)
                {
                    NextMove = Direction.RIGHT;
                }
                else if (StartPosition.X == Matrix.Width - 1 && StartPosition.Y > 0)
                {
                    NextMove = Direction.UP;
                }
                else if (StartPosition.Y == 0 && StartPosition.X > 0)
                {
                    NextMove = Direction.LEFT;
                }
            }

            switch (NextMove)
            {
                case Direction.LEFT: return new List<Point>() { new Point(StartPosition.X - 1, StartPosition.Y) };
                case Direction.RIGHT: return new List<Point>() { new Point(StartPosition.X + 1, StartPosition.Y) };
                case Direction.UP: return new List<Point>() { new Point(StartPosition.X, StartPosition.Y - 1) };
                case Direction.DOWN: return new List<Point>() { new Point(StartPosition.X, StartPosition.Y + 1) };
                default: return new List<Point>();
            }
        }
    }
}

using GameCore.Entities.Interfaces.Algorithms;
using GameCore.Entities.Interfaces.Games;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameCore.Entities.Implements.Algorithms
{
    public class BfsAlgorithm : IPathAlgorithm
    {
        public IMatrix Matrix { get; }
        public List<Point> Goals { get; }
        private Func<Point, bool> CheckMoveablePosition { get;}
        private Func<Point, bool> CheckGoalPosition { get; }

        private VisitedPoint[,] Cells { set; get; }
        private Queue<VisitedPoint> BfsQueue { set; get; }

        public BfsAlgorithm(IMatrix Matrix, Func<Point, bool> CheckMoveablePosition, Func<Point, bool> CheckGoalPosition)
        {
            this.Matrix = Matrix;
            this.CheckMoveablePosition = CheckMoveablePosition;
            this.CheckGoalPosition = CheckGoalPosition;
           
            this.BfsQueue = new Queue<VisitedPoint>();
            this.Goals = new List<Point>();
        }

        public List<Point> FindPath(Point StartPosition)
        {
            this.Cells = new VisitedPoint[Matrix.Width, Matrix.Height];
            this.BfsQueue.Clear();
            this.Goals.Clear();

            for (int Column = 0; Column < Matrix.Width; Column++)
            {
                for (int Row = 0; Row < Matrix.Height; Row++)
                {
                    if (CheckGoalPosition(new Point(Column, Row)))
                        Goals.Add(new Point(Column, Row));

                    if (CheckMoveablePosition(new Point(Column, Row)))
                        Cells[Column, Row] = new VisitedPoint(Column, Row);
                }
            }
            BfsQueue.Enqueue(new VisitedPoint(StartPosition.X, StartPosition.Y));

            while (BfsQueue.Count > 0)
            {
                VisitedPoint Current = BfsQueue.Dequeue();
                if (Goals.Contains(Current.ToPoint()))
                {
                    return GetSolution(Current);
                }
                if (Current != null)
                {
                    GenMove(Current);
                }
            }
            return null;
        }

        private void AddToQueue(VisitedPoint Current, VisitedPoint Previous)
        {
            if (Current != null && !Current.IsVisited)
            {
                Current.Visit(Previous);
                BfsQueue.Enqueue(Current);
            }
        }

        private List<Point> GetSolution(VisitedPoint FinalPoint)
        {
            List<Point> Solution = new List<Point>
                {
                    FinalPoint.ToPoint()
                };
            while (FinalPoint.Previous != null)
            {
                Solution.Add(FinalPoint.Previous.ToPoint());
                FinalPoint = FinalPoint.Previous;
            }
            return Solution;
        }

        private void GenMove(VisitedPoint Current)
        {
            int MaxX = Matrix.Width - 1;
            int MaxY = Matrix.Height - 1;

            if (Current.X < 0 || Current.Y < 0 || Current.X >= MaxX || Current.Y >= MaxY)
                return;

            if (Current.Y > 0) // Get below cell
                AddToQueue(Cells[Current.X, Current.Y - 1], Current);
            if (Current.Y < Matrix.Height - 1) // Get above cell
                AddToQueue(Cells[Current.X, Current.Y + 1], Current);
            if (Current.X < Matrix.Width - 1) // Get right cell
                AddToQueue(Cells[Current.X + 1, Current.Y], Current);
            if (Current.X > 0) // Get left cell
                AddToQueue(Cells[Current.X - 1, Current.Y], Current);
            
        }

        class VisitedPoint
        {
            public int X { private set; get; }
            public int Y { private set; get; }
            public bool IsVisited { private set; get; }
            public VisitedPoint Previous { private set; get; }

            public VisitedPoint(int X, int Y, VisitedPoint Previous)
                : this(X, Y)
            {
                this.Previous = Previous;
            }
            public VisitedPoint(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }
            public void Visit(VisitedPoint Previous)
            {
                this.IsVisited = true;
                this.Previous = Previous;
            }

            public Point ToPoint() => new Point(X, Y);
        }
    }
}

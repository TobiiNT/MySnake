using GameCore.Entities;
using GameCore.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Bussiness
{
    public class BFSAlgorithm
    {
        private VisitedPoint[,] Cells { set; get; }
        private Queue<VisitedPoint> BfsQueue { set; get; }
        private List<Point> Goals { set; get; }

        public BFSAlgorithm(Map Map, Point StartPosition)
        {
            this.Cells = new VisitedPoint[Map.GetWidth(), Map.GetHeight()];
            this.BfsQueue = new Queue<VisitedPoint>();
            this.Goals = new List<Point>();
            for (int Column = 0; Column < Map.GetWidth(); Column++)
            {
                for (int Row = 0; Row < Map.GetHeight(); Row++)
                {
                    if (Map.GetCellType(new Point(Column, Row)) == CellType.FOOD)
                        Goals.Add(new Point(Column, Row));

                    if (Map.GetCellType(new Point(Column, Row)) == CellType.EMPTY ||
                        Map.GetCellType(new Point(Column, Row)) == CellType.FOOD)
                        Cells[Column, Row] = new VisitedPoint(Column, Row);
                }
            }
            Console.WriteLine($"Goals = {Goals.Count}");
            this.BfsQueue.Enqueue(new VisitedPoint(StartPosition.X, StartPosition.Y));
        }

        public List<Point> FindPath()
        {
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
            List<Point> Solution = new List<Point>();
            Solution.Add(FinalPoint.ToPoint());
            while (FinalPoint.Previous != null)
            {
                Solution.Add(FinalPoint.Previous.ToPoint());
                FinalPoint = FinalPoint.Previous;
            }
            return Solution;
        }

        private void GenMove(VisitedPoint Current)
        {
            if (Current.X < Cells.GetLength(0) - 1)
                AddToQueue(Cells[Current.X + 1, Current.Y], Current);
            if (Current.Y < Cells.GetLength(1) - 1)
                AddToQueue(Cells[Current.X, Current.Y + 1], Current);
            if (Current.X > 0)
                AddToQueue(Cells[Current.X - 1, Current.Y], Current);
            if (Current.Y > 0)
                AddToQueue(Cells[Current.X, Current.Y - 1], Current);
        }
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

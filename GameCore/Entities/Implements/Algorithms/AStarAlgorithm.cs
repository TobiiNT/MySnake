using GameCore.Entities.Interfaces.Algorithms;
using GameCore.Entities.Interfaces.Games;
using GameCore.Utilities.DataStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameCore.Entities.Implements.Algorithms
{
    public class AStarAlgorithm : IPathAlgorithm
    {
        public IMatrix Matrix { get; }
        public List<Point> Goals { get; }
        private Func<Point, bool> CheckMoveablePosition { get; }
        private Func<Point, bool> CheckGoalPosition { get; }
        private Func<Point, bool> CheckAvoidPosition { get; }

        private List<Point> AllNodes { get; }

        public AStarAlgorithm(IMatrix Matrix, Func<Point, bool> CheckMoveablePosition, Func<Point, bool> CheckGoalPosition, Func<Point, bool> CheckAvoidPosition)
        {
            this.Matrix = Matrix;
            this.CheckMoveablePosition = CheckMoveablePosition;
            this.CheckGoalPosition = CheckGoalPosition;
            this.CheckAvoidPosition = CheckAvoidPosition;

            this.AllNodes = new List<Point>();
            this.Goals = new List<Point>();
        }

        public List<Point> FindPath(Point StartPosition)
        {
            this.AllNodes.Clear();
            this.Goals.Clear();

            for (int Column = 0; Column < Matrix.Width; Column++)
            {
                for (int Row = 0; Row < Matrix.Height; Row++)
                {
                    if (CheckGoalPosition(new Point(Column, Row)))
                        Goals.Add(new Point(Column, Row));

                    if (CheckMoveablePosition(new Point(Column, Row)))
                        AllNodes.Add(new Point(Column, Row));
                }
            }

            var Frontier = new PriorityQueue<Point, double>();
            Frontier.Enqueue(StartPosition, 0);
            var CameFrom = new Dictionary<Point, Point?>();
            var CostSoFar = new Dictionary<Point, double>();
            CameFrom[StartPosition] = null;
            CostSoFar[StartPosition] = 0;

            Point Goal = Goals.First();

            while (Frontier.Count > 0)
            {
                var Current = Frontier.Dequeue();
                if (Current.X == Goal.X && Current.Y == Goal.Y)
                    break;

                foreach (var Neighbour in GetNeighbours(Current))
                {
                    if (!AllNodes.Contains(Neighbour) || CheckAvoidPosition(Neighbour))
                        continue;

                    double NewCost = CostSoFar[Current] + 1;
                    if (!CostSoFar.ContainsKey(Neighbour) || NewCost < CostSoFar[Neighbour])
                    {
                        CostSoFar[Neighbour] = NewCost;
                        double Priority = NewCost + H(Goal, Neighbour);
                        Frontier.Enqueue(Neighbour, Priority);
                        CameFrom[Neighbour] = Current;
                    }
                }
            }

            var Path = new List<Point>();
            var CurrentStep = Goal;
            Path.Add(CurrentStep);
            while (CameFrom.ContainsKey(CurrentStep))
            {
                CurrentStep = (Point)CameFrom[CurrentStep];
                Path.Add(CurrentStep);
            }

            Path.Reverse();
            return Path;
        }

        private int H(Point Goal, Point Point)
        {
            // Manhattan distance
            return Math.Abs(Goal.X - Point.X) + Math.Abs(Goal.Y - Point.Y);
        }

        private List<Point> GetNeighbours(Point Current)
        {
            // Implement the function that returns valid neighbors for a given point
            var Neighbours = new List<Point>();
            if (Current.X > 0)
                Neighbours.Add(new Point(Current.X - 1, Current.Y));
            if (Current.Y > 0)
                Neighbours.Add(new Point(Current.X, Current.Y - 1));
            if (Current.X < Matrix.Width - 1)
                Neighbours.Add(new Point(Current.X + 1, Current.Y));
            if (Current.Y < Matrix.Height - 1)
                Neighbours.Add(new Point(Current.X, Current.Y + 1));
            return Neighbours;
        }
    }
}

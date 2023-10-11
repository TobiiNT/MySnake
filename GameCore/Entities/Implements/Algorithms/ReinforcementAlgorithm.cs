using GameCore.Entities.Enums;
using GameCore.Entities.Implements.Algorithms.NeuralNetworks;
using GameCore.Entities.Interfaces.Algorithms;
using GameCore.Entities.Interfaces.Games;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameCore.Entities.Implements.Algorithms
{
    public class ReinforcementAlgorithm : IPathAlgorithm
    {
        public IMatrix Matrix { get; }
        public List<Point> Goals { get; }

        public NeuralNetwork NeuralNetwork { get; }

        public ReinforcementAlgorithm(IMatrix Matrix, int NeuronCounts)
        {
            this.Matrix = Matrix;
           
            this.Goals = new List<Point>();

            this.NeuralNetwork = new NeuralNetwork(4, NeuronCounts, 4);
        }

        public List<Point> FindPath(Point StartPosition)
        {
            double[] Inputs = new double[]
            {

            };
            double[] Outputs = NeuralNetwork.FeedForward(Inputs);

            Direction NextMove = (Direction)Array.IndexOf(Outputs, Outputs.Max());

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

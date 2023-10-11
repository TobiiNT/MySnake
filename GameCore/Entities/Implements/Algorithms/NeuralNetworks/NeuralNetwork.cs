using GameCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Entities.Implements.Algorithms.NeuralNetworks
{
    public class NeuralNetwork
    {
        public Level[] Levels { get; }

        public NeuralNetwork(int InputCount, int NeuronCounts, int OutputCount)
        {
            Levels = new Level[NeuronCounts];
            for (int i = 0; i < NeuronCounts; i++)
            {
                Levels[i] = new Level(InputCount, OutputCount);
            }
        }

        public double[] FeedForward(double[] GivenInputs)
        {
            double[] Outputs = Level.FeedForward(GivenInputs, Levels[0]);
            for (int i = 1; i < Levels.Length; i++)
            {
                Outputs = Level.FeedForward(Outputs, Levels[i]);
            }
            return Outputs;
        }

        public void Mutate(double Amount = 1)
        {
            foreach (var Level in Levels)
            {
                for (int i = 0; i < Level.Biases.Length; i++)
                {
                    Level.Biases[i] = Lerp(Level.Biases[i], Randomizer.NextDouble() * 2 - 1, Amount);
                }
                for (int i = 0; i < Level.Weights.Length; i++)
                {
                    for (int j = 0; j < Level.Weights[i].Length; j++)
                    {
                        Level.Weights[i][j] = Lerp(Level.Weights[i][j], Randomizer.NextDouble() * 2 - 1, Amount);
                    }
                }
            }
        }

        private double Lerp(double A, double B, double T)
        {
            return A + T * (B - A);
        }
    }
}

using GameCore.Utilities;
using System;

namespace GameCore.Entities.Implements.Algorithms.NeuralNetworks
{
    public class Level
    {
        public double[] Inputs { get; }
        public double[] Outputs { get; }
        public double[] Biases { get; }
        public double[][] Weights { get; }

        public Level(int InputCount, int OutputCount)
        {
            Inputs = new double[InputCount];
            Outputs = new double[OutputCount];
            Biases = new double[OutputCount];

            Weights = new double[InputCount][];
            for (int i = 0; i < InputCount; i++)
            {
                Weights[i] = new double[OutputCount];
            }

            Randomize();
        }

        private void Randomize()
        {
            for (int i = 0; i < Inputs.Length; i++)
            {
                for (int j = 0; j < Outputs.Length; j++)
                {
                    Weights[i][j] = Randomizer.NextDouble() * 2 - 1;
                }
            }

            for (int i = 0; i < Biases.Length; i++)
            {
                Biases[i] = Randomizer.NextDouble() * 2 - 1;
            }
        }

        public static double[] FeedForward(double[] GivenInputs, Level Level)
        {
            for (int i = 0; i < Level.Inputs.Length; i++)
            {
                Level.Inputs[i] = GivenInputs[i];
            }

            for (int i = 0; i < Level.Outputs.Length; i++)
            {
                double Sum = 0;
                for (int j = 0; j < Level.Inputs.Length; j++)
                {
                    Sum += Level.Inputs[j] * Level.Weights[j][i];
                }

                Level.Outputs[i] = Sum > Level.Biases[i] ? 1 : 0;
            }

            return Level.Outputs;
        }
    }
}

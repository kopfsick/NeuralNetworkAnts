using System;
using System.ComponentModel.Composition;

namespace NeuralNetworkTest.Neural
{
    public interface ISigmoidFunction
    {
        double Apply(double value);
    }

    [Export(typeof (ISigmoidFunction))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SigmoidFunction : ISigmoidFunction
    {
        public double Apply(double value)
        {
            return 1 / (1 + Math.Exp(value));
        }
    }
}
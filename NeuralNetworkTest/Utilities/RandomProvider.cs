using System;
using System.ComponentModel.Composition;

namespace NeuralNetworkTest.Utilities
{
    public interface IRandomProvider
    {
        double NextDouble();
    }

    [Export(typeof(IRandomProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RandomProvider : IRandomProvider
    {
        private readonly Random _random;

        [ImportingConstructor]
        public RandomProvider()
        {
            _random = new Random();
        }

        public double NextDouble()
        {
            return _random.NextDouble();
        }
    }
}
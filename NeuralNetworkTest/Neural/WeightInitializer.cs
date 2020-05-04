using System.ComponentModel.Composition;
using NeuralNetworkTest.Utilities;

namespace NeuralNetworkTest.Neural
{
    [Export(typeof (IWeightInitializer))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class WeightInitializer : IWeightInitializer
    {
        private readonly IRandomProvider _randomProvider;

        [ImportingConstructor]
        public WeightInitializer(IRandomProvider randomProvider)
        {
            _randomProvider = randomProvider;
        }

        public double GetInitialWeight()
        {
            return (_randomProvider.NextDouble()*2)-1;
        }
    }
}
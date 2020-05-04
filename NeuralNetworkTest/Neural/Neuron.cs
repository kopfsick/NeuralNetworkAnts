using System.ComponentModel.Composition;
using System.Linq;
using NeuralNetworkTest.Genetic;

namespace NeuralNetworkTest.Neural
{
    public interface INeuron : IDna
    {
        double[] Weights { get; set; }
        double Bias { get; set; }
        void Setup(int numberOfInputs);
        double Activate(double[] inputs);
    }

    [Export(typeof(INeuron))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Neuron : INeuron
    {
        private readonly IWeightInitializer _weightInitializer;
        private readonly ISigmoidFunction _sigmoidFunction;

        [ImportingConstructor]
        public Neuron(IWeightInitializer weightInitializer, ISigmoidFunction sigmoidFunction)
        {
            _weightInitializer = weightInitializer;
            _sigmoidFunction = sigmoidFunction;
        }

        public double[] Weights { get; set; }

        public double Bias { get; set; }

        public void Setup(int numberOfInputs)
        {
            Weights = new double[numberOfInputs];

            for (int i = 0; i < Weights.Length; i++)
                Weights[i] = _weightInitializer.GetInitialWeight();

            Bias = _weightInitializer.GetInitialWeight();
        }

        public double Activate(double[] inputs)
        {
            double value = 0;

            for (int i = 0; i < inputs.Length; i++)
                value += Weights[i]*inputs[i];

            value -= Bias;

            value = _sigmoidFunction.Apply(value);

            return value;
        }

        public double[] GetDna()
        {
            return Weights.Concat(new[] {Bias}).ToArray();
        }

        public void SetDna(double[] dna)
        {
            Bias = dna.Last();
            Weights = dna.Take(Weights.Length).ToArray();
        }
    }
}
using System.ComponentModel.Composition;
using System.Linq;
using NeuralNetworkTest.Genetic;
using NeuralNetworkTest.Utilities;

namespace NeuralNetworkTest.Neural
{
    public interface INeuronLayer : IDna
    {
        void Setup(int numberOfNeurons);
        INeuron[] Neurons { get; set; }
        void ConnectTo(INeuronLayer layerToConnectTo);
        double[] Activate(double[] input);
    }

    [Export(typeof (INeuronLayer))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class NeuronLayer : INeuronLayer
    {
        private readonly IObjectFactory<INeuron> _neuronFactory;

        [ImportingConstructor]
        public NeuronLayer(IObjectFactory<INeuron> neuronFactory)
        {
            _neuronFactory = neuronFactory;
        }

        public INeuron[] Neurons { get; set; }

        public void Setup(int numberOfNeurons)
        {
            Neurons = new INeuron[numberOfNeurons];

            for (int i = 0; i < numberOfNeurons; i++)
                Neurons[i] = _neuronFactory.CreateInstance();
        }

        public void ConnectTo(INeuronLayer layerToConnectTo)
        {
            foreach (var neuron in Neurons)
                neuron.Setup(layerToConnectTo.Neurons.Length);
        }

        public double[] Activate(double[] input)
        {
            return Neurons.Select(n => n.Activate(input)).ToArray();
        }

        public double[] GetDna()
        {
            return Neurons.SelectMany(n => n.GetDna()).ToArray();
        }

        public void SetDna(double[] dna)
        {
            var index = 0;
            foreach (var neuron in Neurons)
            {
                neuron.SetDna(dna.Skip(index).Take(neuron.GetDna().Length).ToArray());
                index += neuron.GetDna().Length;
            }
        }
    }
}
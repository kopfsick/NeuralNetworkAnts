using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NeuralNetworkTest.Genetic;
using NeuralNetworkTest.Utilities;

namespace NeuralNetworkTest.Neural
{
    public interface INeuralNetwork :IDna
    {
        INeuronLayer OutputLayer { get; set; }
        INeuronLayer InputLayer { get; set; }
        INeuronLayer[] HiddenLayers { get; set; }
        void Setup(int numberOfInputs, int numberOfOutputs, params int[] numberOfNeuronsInHiddenLayer);
        double[] GetOutput(params double[] input);
    }

    [Export(typeof (INeuralNetwork))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class NeuralNetwork : INeuralNetwork
    {
        private readonly IObjectFactory<INeuronLayer> _neuronLayerFactory;

        [ImportingConstructor]
        public NeuralNetwork(IObjectFactory<INeuronLayer> neuronLayerFactory)
        {
            _neuronLayerFactory = neuronLayerFactory;
        }

        public INeuronLayer OutputLayer { get; set; }

        public INeuronLayer InputLayer { get; set; }

        public INeuronLayer[] HiddenLayers { get; set; }

        public void Setup(int numberOfInputs, int numberOfOutputs, params int[] numberOfNeuronsInHiddenLayer)
        {
            var hiddenLayers = new List<INeuronLayer>(numberOfNeuronsInHiddenLayer.Length);

            InputLayer = _neuronLayerFactory.CreateInstance();
            InputLayer.Setup(numberOfInputs);

            OutputLayer = _neuronLayerFactory.CreateInstance();
            OutputLayer.Setup(numberOfOutputs);

            var layerToConnectTo = InputLayer;

            foreach (var numberOfHiddenNeurons in numberOfNeuronsInHiddenLayer)
            {
                var hiddenLayer = _neuronLayerFactory.CreateInstance();
                hiddenLayer.Setup(numberOfHiddenNeurons);
                hiddenLayer.ConnectTo(layerToConnectTo);

                hiddenLayers.Add(hiddenLayer);

                layerToConnectTo = hiddenLayer;
            }

            OutputLayer.ConnectTo(layerToConnectTo);

            HiddenLayers = hiddenLayers.ToArray();
        }

        public double[] GetOutput(params double[] input)
        {
            var layerInput = input;

            foreach (var layer in HiddenLayers)
                layerInput = layer.Activate(layerInput);

            return OutputLayer.Activate(layerInput);
        }

        public double[] GetDna()
        {
            return HiddenLayers.SelectMany(l => l.GetDna()).Concat(OutputLayer.GetDna()).ToArray();
        }

        public void SetDna(double[] dna)
        {
            var index = 0;
            foreach (var layer in HiddenLayers.Concat(new [] {OutputLayer}))
            {
                layer.SetDna(dna.Skip(index).Take(layer.GetDna().Length).ToArray());
                index += layer.GetDna().Length;
            }
        }
    }
}
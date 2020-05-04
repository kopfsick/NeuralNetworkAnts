using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using NeuralNetworkTest.Genetic;
using NeuralNetworkTest.Neural;
using NeuralNetworkTest.Utilities;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.Creature
{
    public interface ICreature : IFitDna, IWorldObject
    {
        void Setup(int numberOfInputsToExpect, double worldWidth, double worldHeight);
    }

    [Export(typeof (ICreature))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Creature : ICreature
    {
        private readonly INeuralNetwork _neuralNetwork;
        private readonly IRandomProvider _randomProvider;
        private double _angle;
        private double _worldHeight;
        private double _worldWidth;

        private const double MAX_ANGLE_CHANGE = 60;
        private const double SPEED_FACTOR = 2;
        private const double ANGLE_FACTOR = 120;

        [ImportingConstructor]
        public Creature(INeuralNetwork neuralNetwork, IRandomProvider randomProvider)
        {
            _neuralNetwork = neuralNetwork;
            _randomProvider = randomProvider;
        }

        public double Angle
        {
            get { return _angle; }
            set { _angle = value < 0 ? 360 - value : value % 360; }
        }

        public Vector Position { get; set; }

        public double Fitness { get; set; }

        public void Setup(int numberOfInputsToExpect, double worldWidth, double worldHeight)
        {
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;

            const int numberOfOutputs = 2;
            var hiddenLayers = new []{10};

            _neuralNetwork.Setup(numberOfInputsToExpect+2, numberOfOutputs, hiddenLayers);

            ResetPostion();
        }

        public void ResetPostion()
        {
            Position = new Vector(_randomProvider.NextDouble()*_worldWidth, _randomProvider.NextDouble()*_worldHeight);
//            Position = new Vector(250,250);
            Angle = _randomProvider.NextDouble()*360d;
            Updated.Invoke();
        }

        public double Width
        {
            get { return 10; }
        }

        public void Update(double[] inputs)
        {
            var angleRadians = Angle*(Math.PI/180.0);
            var directionX = Math.Sin(angleRadians);
            var directionY = (-1d)*Math.Cos(angleRadians);
            var direction = new Vector(directionX, directionY);

            var outputs = _neuralNetwork.GetOutput(inputs.Concat(new[] {directionX, directionY}).ToArray());

            var speedLeft = outputs[0];
            var speedRight = outputs[1];

            if(double.IsNaN(speedLeft) || double.IsNaN(speedLeft))
                return;

            var requestedAngle = (speedLeft - speedRight) * ANGLE_FACTOR;
            requestedAngle = Math.Max(-1*MAX_ANGLE_CHANGE, requestedAngle);
            requestedAngle = Math.Min(MAX_ANGLE_CHANGE, requestedAngle);
            Angle += requestedAngle;

            var speed = speedLeft + speedRight;
            var movement = Vector.Multiply(direction, speed * SPEED_FACTOR);

            var position = Vector.Add(Position, movement);
            position.X = position.X > 0 ? position.X%_worldWidth : _worldWidth - position.X;
            position.Y = position.Y > 0 ? position.Y % _worldHeight : _worldHeight - position.Y;
            Position = position;

            Updated.Invoke();
        }

        public double[] GetDna()
        {
            return _neuralNetwork.GetDna();
        }

        public void SetDna(double[] dna)
        {
            _neuralNetwork.SetDna(dna);
            ResetPostion();
        }

        public string Speicies
        {
            get { return GetType().ToString(); }
        }

        public event Action Updated = delegate { };
    }
}
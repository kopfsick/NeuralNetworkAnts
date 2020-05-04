using System;
using System.ComponentModel.Composition;
using System.Windows;
using NeuralNetworkTest.Utilities;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.VariableFood
{
    public interface IVariableFood : IWorldObject
    {
        void Setup(double worldWidth, double worldHeight);
        void EatSome();
    }

    [Export(typeof(IVariableFood))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class VariableFood : IVariableFood
    {
        private readonly IRandomProvider _randomProvider;
        private double _worldWidth;
        private double _worldHeight;
        private double _foodAmount;

        [ImportingConstructor]
        public VariableFood(IRandomProvider randomProvider)
        {
            _randomProvider = randomProvider;
        }

        public string Speicies
        {
            get { return GetType().ToString(); }
        }

        public Vector Position { get; set; }

        public double Width
        {
            get { return _foodAmount; }
        }

        public double Angle
        {
            get { return 0; }
        }

        public void Update(double[] inputs)
        {}

        public void ResetPostion()
        {
            Position = new Vector(_randomProvider.NextDouble() * _worldWidth, _randomProvider.NextDouble() * _worldHeight);
            _foodAmount = (_randomProvider.NextDouble()*45) + 5;
            Updated.Invoke();
        }

        public void Setup(double worldWidth, double worldHeight)
        {
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
            ResetPostion();
        }

        public void EatSome()
        {
            _foodAmount -= 1;
            if(_foodAmount < 1)
                ResetPostion();
            else
                Updated.Invoke();
        }

        public event Action Updated = delegate { };
    }
}
using System;
using System.ComponentModel.Composition;
using System.Windows;
using NeuralNetworkTest.Utilities;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.Food
{
    public interface IFood : IWorldObject
    {
        void Setup(double worldWidth, double worldHeight);
    }

    [Export(typeof (IFood))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Food : IFood
    {
        private readonly IRandomProvider _randomProvider;
        private double _worldWidth;
        private double _worldHeight;

        [ImportingConstructor]
        public Food(IRandomProvider randomProvider)
        {
            _randomProvider = randomProvider;
        }

        public string Speicies
        {
            get { return "Food"; }
        }

        public Vector Position { get; set; }

        public double Width
        {
            get { return 5; }
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
            Updated.Invoke();
        }

        public void Setup(double worldWidth, double worldHeight)
        {
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
            ResetPostion();
        }

        public event Action Updated = delegate { };
    }
}
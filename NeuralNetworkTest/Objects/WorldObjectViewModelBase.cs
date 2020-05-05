using NeuralNetworkTest.Utilities;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects
{
    public interface IWorldObjectViewModelBase<in TObject> where TObject : IWorldObject
    {
        void Setup(TObject o);
        double Width { get; }
        double Angle { get; }
        double Y { get; }
        double X { get; }
    }

    public class WorldObjectViewModelBase<TObject> : ViewModelBase, IWorldObjectViewModelBase<TObject> where TObject : IWorldObject
    {
        private TObject _object;

        public void Setup(TObject o)
        {
            _object = o;
            Update();
            _object.Updated += Update;
        }

        private void Update()
        {
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(Angle));
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
        }

        public double Width => _object.Width;
        public double Angle => _object.Angle;
        public double Y => _object.Position.Y;
        public double X => _object.Position.X;
    }
}
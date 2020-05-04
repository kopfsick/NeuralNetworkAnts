using System.ComponentModel;
using NeuralNetworkTest.Utilities;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects
{
    public interface IWorldObjectViewModelBase<in TObject> where TObject : IWorldObject
    {
        void Setup(TObject o);
        double Width { get; set; }
        double Angle { get; set; }
        double Y { get; set; }
        double X { get; set; }
    }

    public class WorldObjectViewModelBase<TObject> : ViewModelBase, IWorldObjectViewModelBase<TObject> where TObject : IWorldObject
    {
        private double _x;
        private double _y;
        private double _angle;
        private double _width;
        private TObject _object;

        public void Setup(TObject o)
        {
            _object = o;
            Update();
            _object.Updated += Update;
        }

        private void Update()
        {
            X = _object.Position.X;
            Y = _object.Position.Y;
            Angle = _object.Angle;
            Width = _object.Width;
        }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                OnPropertyChanged();
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged();
            }
        }
    }
}
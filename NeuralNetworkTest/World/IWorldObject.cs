using System;
using System.Windows;

namespace NeuralNetworkTest.World
{
    public interface IWorldObject
    {
        string Speicies { get; }
        Vector Position { get; }
        double Width { get; }
        double Angle { get; }
        void Update(double[] inputs);
        void ResetPostion();
        event Action Updated;
    }
}
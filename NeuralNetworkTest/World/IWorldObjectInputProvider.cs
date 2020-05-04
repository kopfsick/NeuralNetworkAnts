namespace NeuralNetworkTest.World
{
    public interface IWorldObjectInputProvider
    {
        bool Accepts(IWorldObject worldObject);
        double[] CreateInputs(IWorld world, IWorldObject worldObject);
    }
}
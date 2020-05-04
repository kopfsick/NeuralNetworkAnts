namespace NeuralNetworkTest.World
{
    public interface IWorldObjectFactory
    {
        IWorldObject[] CreateObjects(double worldWidth, double worldHeight);
    }
}
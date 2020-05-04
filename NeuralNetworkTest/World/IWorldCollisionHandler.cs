namespace NeuralNetworkTest.World
{
    public interface IWorldCollisionHandler
    {
        bool CanHandle(IWorldObject item1, IWorldObject item2);
        void Handle(IWorldObject item1, IWorldObject item2);
    }
}
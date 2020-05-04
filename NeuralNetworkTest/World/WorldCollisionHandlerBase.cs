namespace NeuralNetworkTest.World
{
    public abstract class WorldCollisionHandlerBase<TItem1,TItem2> where TItem1 : class, IWorldObject where TItem2: class, IWorldObject
    {
        public bool CanHandle(IWorldObject item1, IWorldObject item2)
        {
            return (item1 is TItem2 && item2 is TItem1) || (item2 is TItem2 && item1 is TItem1);
        }

        public void Handle(IWorldObject item1, IWorldObject item2)
        {
            var creature = (item1 as TItem1) ?? (item2 as TItem1);
            var food = (item1 as TItem2) ?? (item2 as TItem2);

            PerformCollision(creature, food);
        }

        protected abstract void PerformCollision(TItem1 item1, TItem2 item2);
    }
}
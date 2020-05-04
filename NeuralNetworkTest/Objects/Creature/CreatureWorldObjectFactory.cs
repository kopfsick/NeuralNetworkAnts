using System.ComponentModel.Composition;
using System.Linq;
using NeuralNetworkTest.Utilities;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.Creature
{
    [Export(typeof(IWorldObjectFactory))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CreatureWorldObjectFactory : IWorldObjectFactory
    {
        private readonly IObjectFactory<ICreature> _creatureFactory;

        [ImportingConstructor]
        public CreatureWorldObjectFactory(IObjectFactory<ICreature> creatureFactory)
        {
            _creatureFactory = creatureFactory;
        }

        public IWorldObject[] CreateObjects(double worldWidth, double worldHeight)
        {
            var creatures = Enumerable.Range(0, 20).Select(i =>
            {
                var creature = _creatureFactory.CreateInstance();
                creature.Setup(4, worldWidth, worldHeight);
                return (IWorldObject) creature;
            });

            return creatures.ToArray();
        }
    }
}
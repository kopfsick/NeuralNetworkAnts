using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using NeuralNetworkTest.Genetic;
using NeuralNetworkTest.Objects.Creature;
using NeuralNetworkTest.Objects.Food;
using NeuralNetworkTest.Objects.VariableFood;

namespace NeuralNetworkTest.World
{
    public interface IWorld
    {
        void Update();
        void Setup(double width, double height);
        IEnumerable<IEnumerable<IFitDna>> Populations { get; }
        double Height { get; set; }
        double Width { get; set; }
        ObservableCollection<IWorldObject> WorldObjects { get; set; }
        event Action Initialized;
        void ResetObjectPositions();
        event Action Updated;
    }

    [Export(typeof(IWorld))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class World : IWorld
    {
        private readonly IWorldObjectFactory[] _worldObjectFactories;
        private readonly IWorldCollisionHandler[] _collisionHandlers;
        private readonly IWorldObjectInputProvider[] _worldObjectInputProviders;

        [ImportingConstructor]
        public World([ImportMany]IWorldObjectFactory[] worldObjectFactories, [ImportMany]IWorldCollisionHandler[] collisionHandlers, [ImportMany]IWorldObjectInputProvider[] worldObjectInputProviders)
        {
            _worldObjectFactories = worldObjectFactories;
            _worldObjectInputProviders = worldObjectInputProviders;
            _collisionHandlers = collisionHandlers.ToArray();
            WorldObjects = new ObservableCollection<IWorldObject>();
        }

        public ObservableCollection<IWorldObject> WorldObjects { get; set; }

        public void Update()
        {
            Parallel.ForEach(WorldObjects, worldObject =>
            {
                worldObject.Update(_worldObjectInputProviders.Where(provider => provider.Accepts(worldObject)).SelectMany(provider => provider.CreateInputs(this, worldObject)).ToArray());
            });

            var collisions = FindCollisions();

            Parallel.ForEach(collisions, collision =>
            {
                var theCollision = collision;
                var handlers = _collisionHandlers.Where(h => h.CanHandle(theCollision.Item1, theCollision.Item2));
                foreach (var handler in handlers)
                    handler.Handle(collision.Item1, collision.Item2);
            });

            Updated.Invoke();
        }

        private Tuple<IWorldObject,IWorldObject>[] FindCollisions()
        {
            var collisions = new ConcurrentBag<Tuple<IWorldObject,IWorldObject>>();

            var otherWorldObjects = WorldObjects.OfType<IFood>().Cast<IWorldObject>().Concat(WorldObjects.OfType<IVariableFood>()).ToArray();
            var worldObjectsToCheck = WorldObjects.OfType<ICreature>();
            Parallel.ForEach(worldObjectsToCheck, worldObject =>
            {
                Parallel.ForEach(otherWorldObjects, otherWorldObject =>
                {
                    if (worldObject != otherWorldObject &&
                        Math.Abs((worldObject.Position - otherWorldObject.Position).Length - (worldObject.Width/2) -
                                 (otherWorldObject.Width/2)) < 5)
                        if (
                            !collisions.Any(
                                c =>
                                    (c.Item1 == worldObject && c.Item2 == otherWorldObject) ||
                                    (c.Item2 == worldObject && c.Item1 == otherWorldObject)))
                            collisions.Add(new Tuple<IWorldObject, IWorldObject>(worldObject, otherWorldObject));
                });
            });

            return collisions.ToArray();
        }

        public void Setup(double width, double height)
        {
            Width = width;
            Height = height;

            WorldObjects.Clear();

            foreach (var factory in _worldObjectFactories)
            {
                var newObjects = factory.CreateObjects(Width, Height);
                foreach (var newObject in newObjects)
                    WorldObjects.Add(newObject);
            }

            Initialized.Invoke();
        }

        public void ResetObjectPositions()
        {
            foreach (var worldObject in WorldObjects)
                worldObject.ResetPostion();
        }

        public double Height { get; set; }

        public double Width { get; set; }

        public IEnumerable<IEnumerable<IFitDna>> Populations
        {
            get { return WorldObjects.OfType<IFitDna>().GroupBy(o => ((IWorldObject) o).Speicies); }
        }

        public event Action Initialized = delegate { };
        public event Action Updated = delegate { };
    }
}
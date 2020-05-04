using System.Linq;
using System.Windows;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.Creature
{
    public class DirectionCreatureWorldObjectInputProviderBase<TFood> : IWorldObjectInputProvider where TFood : IWorldObject
    {
        public bool Accepts(IWorldObject worldObject)
        {
            return worldObject is ICreature;
        }

        public double[] CreateInputs(IWorld world, IWorldObject worldObject)
        {
            var creature = worldObject as ICreature;
            var foods = world.WorldObjects.OfType<TFood>();

            var closestFood = new Vector(double.MaxValue, double.MaxValue);
            double closestFoodDistance = double.MaxValue;

            foreach (var food in foods)
            {
                var directional = Vector.Subtract(creature.Position, food.Position);
                var distanceToCurrentFood = directional.Length - food.Width/2 - creature.Width / 2;
                if (distanceToCurrentFood < closestFoodDistance)
                {
                    closestFood = directional;
                    closestFoodDistance = distanceToCurrentFood;
                }
            }

            closestFood = Vector.Divide(closestFood, closestFood.Length);
            return new[] {closestFood.X, closestFood.Y};
        }
    }
}
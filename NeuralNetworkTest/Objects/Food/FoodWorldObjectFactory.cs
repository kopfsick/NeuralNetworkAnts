using System.ComponentModel.Composition;
using System.Linq;
using NeuralNetworkTest.Utilities;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.Food
{
//    [Export(typeof(IWorldObjectFactory))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class FoodWorldObjectFactory : IWorldObjectFactory
    {
        private readonly IObjectFactory<IFood> _foodFactory;

        [ImportingConstructor]
        public FoodWorldObjectFactory(IObjectFactory<IFood> foodFactory)
        {
            _foodFactory = foodFactory;
        }

        public IWorldObject[] CreateObjects(double worldWidth, double worldHeight)
        {
            var foods = Enumerable.Range(0, 15).Select(i =>
            {
                var food = _foodFactory.CreateInstance();
                food.Setup(worldWidth, worldHeight);
                return (IWorldObject)food;
            });

            return foods.ToArray();
        }
    }
}
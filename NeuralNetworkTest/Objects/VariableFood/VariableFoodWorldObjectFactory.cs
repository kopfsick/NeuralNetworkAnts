using System.ComponentModel.Composition;
using System.Linq;
using NeuralNetworkTest.Utilities;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.VariableFood
{
    [Export(typeof(IWorldObjectFactory))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class VariableFoodWorldObjectFactory : IWorldObjectFactory
    {
        private readonly IObjectFactory<IVariableFood> _foodFactory;

        [ImportingConstructor]
        public VariableFoodWorldObjectFactory(IObjectFactory<IVariableFood> foodFactory)
        {
            _foodFactory = foodFactory;
        }

        public IWorldObject[] CreateObjects(double worldWidth, double worldHeight)
        {
            var foods = Enumerable.Range(0, 10).Select(i =>
            {
                var food = _foodFactory.CreateInstance();
                food.Setup(worldWidth, worldHeight);
                return (IWorldObject)food;
            });

            return foods.ToArray();
        }
    }
}
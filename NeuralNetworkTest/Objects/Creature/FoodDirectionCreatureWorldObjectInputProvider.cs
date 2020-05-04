using System.ComponentModel.Composition;
using NeuralNetworkTest.Objects.Food;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.Creature
{
//    [Export(typeof (IWorldObjectInputProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class FoodDirectionCreatureWorldObjectInputProvider : DirectionCreatureWorldObjectInputProviderBase<IFood>
    {
    }
}
using System.ComponentModel.Composition;
using NeuralNetworkTest.Objects.VariableFood;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.Creature
{
    [Export(typeof(IWorldObjectInputProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class VariableFoodDirectionCreatureWorldObjectInputProvider : DirectionCreatureWorldObjectInputProviderBase<IVariableFood>
    {
    }
}
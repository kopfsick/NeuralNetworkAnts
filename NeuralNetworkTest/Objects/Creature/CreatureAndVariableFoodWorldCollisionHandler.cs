using System.ComponentModel.Composition;
using NeuralNetworkTest.Objects.VariableFood;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.Creature
{
    [Export(typeof(IWorldCollisionHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CreatureAndVariableFoodWorldCollisionHandler : WorldCollisionHandlerBase<ICreature,IVariableFood>, IWorldCollisionHandler
    {
        protected override void PerformCollision(ICreature item1, IVariableFood item2)
        {
            item1.Fitness += 1;
            item2.EatSome();
        }
    }
}
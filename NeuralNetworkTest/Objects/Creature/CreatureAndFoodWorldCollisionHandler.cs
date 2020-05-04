using System.ComponentModel.Composition;
using NeuralNetworkTest.Objects.Food;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest.Objects.Creature
{
//    [Export(typeof (IWorldCollisionHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CreatureAndFoodWorldCollisionHandler : WorldCollisionHandlerBase<ICreature, IFood>, IWorldCollisionHandler
    {
        protected override void PerformCollision(ICreature item1, IFood item2)
        {
            item1.Fitness -= item1.Fitness/2d;
            item2.ResetPostion();
        }
    }
}
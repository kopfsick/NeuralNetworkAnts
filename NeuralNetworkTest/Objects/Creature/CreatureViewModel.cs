using System.ComponentModel.Composition;

namespace NeuralNetworkTest.Objects.Creature
{
    public interface ICreatureViewModel : IWorldObjectViewModelBase<ICreature>
    {}

    [Export(typeof(ICreatureViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CreatureViewModel : WorldObjectViewModelBase<ICreature>, ICreatureViewModel
    {}
}
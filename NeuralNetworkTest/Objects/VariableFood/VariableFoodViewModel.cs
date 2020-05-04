using System.ComponentModel.Composition;

namespace NeuralNetworkTest.Objects.VariableFood
{
    public interface IVariableFoodViewModel : IWorldObjectViewModelBase<IVariableFood>
    { }

    [Export(typeof(IVariableFoodViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class VariableFoodViewModel : WorldObjectViewModelBase<IVariableFood>, IVariableFoodViewModel
    { }
}
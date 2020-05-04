using System.ComponentModel.Composition;
using NeuralNetworkTest.Utilities;

namespace NeuralNetworkTest.Objects.Food
{
    public interface IFoodViewModel : IWorldObjectViewModelBase<IFood>
    {}

    [Export(typeof(IFoodViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FoodViewModel : WorldObjectViewModelBase<IFood>, IFoodViewModel
    {}
}
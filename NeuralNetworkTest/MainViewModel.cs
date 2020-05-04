using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NeuralNetworkTest.Utilities;

namespace NeuralNetworkTest
{
    public interface IMainViewModel
    {
        string Text { get; set; }
        IMainViewModelPart[] PartViewModels { get; }
    }

    [Export(typeof(IMainViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IMainViewModelPart[] _partViewModels;

        [ImportingConstructor]
        public MainViewModel([ImportMany]IMainViewModelPart[] partViewModels)
        {
            _partViewModels = partViewModels.OrderBy(p => p.Rank).ToArray();
            Text = "Hallo du";
        }

        public string Text { get; set; }

        public IMainViewModelPart[] PartViewModels
        {
            get { return _partViewModels; }
        }
    }

    public interface IMainViewModelPart
    {
        int Rank { get; }
        Dock DockLocation { get; }
    }

    
}

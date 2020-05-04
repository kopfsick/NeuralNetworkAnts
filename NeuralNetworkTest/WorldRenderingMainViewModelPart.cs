using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using NeuralNetworkTest.Objects.Creature;
using NeuralNetworkTest.Objects.Food;
using NeuralNetworkTest.Objects.VariableFood;
using NeuralNetworkTest.Utilities;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest
{
    [Export(typeof(IMainViewModelPart))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class WorldRenderingMainViewModelPart :ViewModelBase, IMainViewModelPart
    {
        private readonly IWorld _world;
        private readonly IObjectFactory<ICreatureViewModel> _creatureViewModelFactory;
        private readonly IObjectFactory<IFoodViewModel> _foodViewModelFactory;
        private readonly IObjectFactory<IVariableFoodViewModel> _variableFoodViewModelFactory;
        private double _worldWidth;
        private double _worldHeight;

        [ImportingConstructor]
        public WorldRenderingMainViewModelPart(IWorld world, IObjectFactory<ICreatureViewModel> creatureViewModelFactory, IObjectFactory<IFoodViewModel> foodViewModelFactory, IObjectFactory<IVariableFoodViewModel> variableFoodViewModelFactory)
        {
            Objects = new ObservableCollection<object>();
            _world = world;
            _creatureViewModelFactory = creatureViewModelFactory;
            _foodViewModelFactory = foodViewModelFactory;
            _variableFoodViewModelFactory = variableFoodViewModelFactory;
            Setup();
            _world.Initialized += Setup;
        }

        public double WorldWidth
        {
            get { return _worldWidth; }
            set
            {
                _worldWidth = value;
                OnPropertyChanged();
            }
        }

        public double WorldHeight
        {
            get { return _worldHeight; }
            set
            {
                _worldHeight = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<object> Objects { get; set; }

        private void Setup()
        {
            WorldHeight = _world.Height;
            WorldWidth = _world.Width;

            Objects.Clear();

            foreach (var creature in _world.WorldObjects.OfType<ICreature>())
            {
                var vm = _creatureViewModelFactory.CreateInstance();
                vm.Setup(creature);
                Objects.Add(vm);
            }

            foreach (var food in _world.WorldObjects.OfType<IFood>())
            {
                var vm = _foodViewModelFactory.CreateInstance();
                vm.Setup(food);
                Objects.Add(vm);
            }

            foreach (var food in _world.WorldObjects.OfType<IVariableFood>())
            {
                var vm = _variableFoodViewModelFactory.CreateInstance();
                vm.Setup(food);
                Objects.Add(vm);
            }
        }

        public int Rank { get { return 7000; } }
        public Dock DockLocation { get { return Dock.Bottom; } }
    }
}
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using NeuralNetworkTest.Utilities;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest
{
    [Export(typeof (IMainViewModelPart))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ControlsMainViewModelPart : ViewModelBase, IMainViewModelPart
    {
        private IMainController _mainController;

        [ImportingConstructor]
        public ControlsMainViewModelPart(IMainController mainController)
        {
            _mainController = mainController;
        }

        public bool Fast
        {
            get { return _mainController.Fast; }
            set
            {
                _mainController.Fast = value;
                OnPropertyChanged();
            }
        }

        public int Rank
        {
            get { return 2000; }
        }

        public Dock DockLocation
        {
            get { return Dock.Top; }
        }
    }

    [Export(typeof (IMainViewModelPart))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class StatsMainViewModelPart : ViewModelBase, IMainViewModelPart
    {
        private readonly IMainController _mainController;
        private readonly IWorld _world;
        private int _generationCount;
        private int _updatesSinceLastGeneration;
        private string _bestFitness;
        private string _averageFitness;

        [ImportingConstructor]
        public StatsMainViewModelPart(IMainController mainController, IWorld world)
        {
            _mainController = mainController;
            _world = world;

            MainControllerUpdate();
            _mainController.Updated += MainControllerUpdate;

            WorldUpdate();
            _world.Updated += WorldUpdate;
        }

        private void WorldUpdate()
        {
            BestFitness = String.Join(", ", _world.Populations.Select(population => population.Max(o => o.Fitness)));
            AverageFitness = String.Join(", ", _world.Populations.Select(population => population.Average(o => o.Fitness)));
        }

        public string AverageFitness
        {
            get { return _averageFitness; }
            set
            {
                _averageFitness = value;
                OnPropertyChanged();
            }
        }

        public string BestFitness
        {
            get { return _bestFitness; }
            set
            {
                _bestFitness = value;
                OnPropertyChanged();
            }
        }

        private void MainControllerUpdate()
        {
            GenerationCount = _mainController.GenerationCount;
            UpdatesSinceLastGeneration = _mainController.UpdatesSinceLastEpoch;
        }

        public int UpdatesSinceLastGeneration
        {
            get { return _updatesSinceLastGeneration; }
            private set
            {
                _updatesSinceLastGeneration = value;
                OnPropertyChanged();
            }
        }

        public int GenerationCount
        {
            private get { return _generationCount; }
            set
            {
                _generationCount = value;
                OnPropertyChanged();
            }
        }

        public int Rank
        {
            get { return 1000; }
        }

        public Dock DockLocation
        {
            get { return Dock.Right; }
        }
    }
}
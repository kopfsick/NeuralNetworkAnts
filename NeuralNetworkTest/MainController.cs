using System;
using System.Collections;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using NeuralNetworkTest.Genetic;
using NeuralNetworkTest.World;

namespace NeuralNetworkTest
{
    public interface IMainController
    {
        void Setup();
        void Start();
        void Pause();
        event Action Updated;
        int UpdatesSinceLastEpoch { get; }
        int GenerationCount { get; set; }
        bool Fast { get; set; }
    }

    [Export(typeof(IMainController))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainController : IMainController
    {
        private readonly IWorld _world;
        private readonly IEvolution _evolution;

        private DispatcherTimer _timer;
        private int _updatesSinceLastEpoch;
        private int _generationCount;

        private const int MAX_UPDATES_PER_EPOCH = 1000;
        private const int UPDATE_INTERVAL = 1;

        [ImportingConstructor]
        public MainController(IWorld world, IEvolution evolution)
        {
            _world = world;
            _evolution = evolution;
        }

        public int UpdatesSinceLastEpoch
        {
            get { return _updatesSinceLastEpoch; }
        }

        public int GenerationCount
        {
            get { return _generationCount; }
            set { _generationCount = value; }
        }

        public void Setup()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Normal);
            _timer.Tick += OnDispatcherTimerTick;
            _timer.Interval = TimeSpan.FromMilliseconds(UPDATE_INTERVAL);

            _world.Setup(650, 650);
        }

        private void UpdateGenerationCounters()
        {
            _updatesSinceLastEpoch = 0;
            _generationCount++;
        }

        private void OnDispatcherTimerTick(object sender, EventArgs e)
        {
            if (Fast)
            {
                _timer.Stop();
                Start();
                return;
            }
            Update();
        }

        private void Update()
        {
            _world.Update();
            _updatesSinceLastEpoch++;

            if (ShouldEpoch())
                Epoch();

            Updated.Invoke();
        }

        private bool ShouldEpoch()
        {
            return _updatesSinceLastEpoch > MAX_UPDATES_PER_EPOCH;
        }

        private void Epoch()
        {
            foreach (var population in _world.Populations)
            {
                _evolution.Evolve(population.ToArray());
                foreach (var fitDna in population)
                    fitDna.Fitness = 0;
            }
                

            _world.ResetObjectPositions();

            UpdateGenerationCounters();
        }

        public void Start()
        {
            if(!Fast)
                _timer.Start();

            else
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        if (!Fast)
                        {
                            _timer.Start();
                            break;
                        }

                        Update();
                    }
                });
            }
        }

        public bool Fast { get; set; }

        public void Pause()
        {
            _timer.Stop();
        }

        public event Action Updated = delegate { };
    }
}
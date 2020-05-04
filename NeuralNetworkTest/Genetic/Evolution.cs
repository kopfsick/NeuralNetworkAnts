using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NeuralNetworkTest.Utilities;

namespace NeuralNetworkTest.Genetic
{
    public interface IEvolution
    {
        void Evolve(IFitDna[] population);
    }

    [Export(typeof(IEvolution))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Evolution : IEvolution
    {
        private readonly IRandomProvider _randomProvider;
        private double ELITE_PERCENTAGE = 0.2;
        private double CROSSOVER_RATE = 0.8;
        private double MUTATION_RATE = 0.05;

        [ImportingConstructor]
        public Evolution(IRandomProvider randomProvider)
        {
            _randomProvider = randomProvider;
        }

        public void Evolve(IFitDna[] population)
        {
            var ordered = population.OrderByDescending(p => p.Fitness).ToArray();

            var eliteCount = (int)Math.Floor(ELITE_PERCENTAGE*population.Length);

            var newDnas = new List<double[]>();

            for (int i = eliteCount; i < ordered.Length; i++)
            {
                var parent1 = RouletteWheelSelection(ordered);

                if (_randomProvider.NextDouble() < CROSSOVER_RATE)
                {
                    var parent2 = RouletteWheelSelection(ordered);

                    var childDna = Crossover(parent1, parent2);
                    Mutate(childDna, MUTATION_RATE);

                    newDnas.Add(childDna);
                }
                else
                {
                    newDnas.Add(parent1.GetDna());
                }
                
            }

            for (int i = eliteCount; i < ordered.Length; i++)
            {
                ordered[i].SetDna(newDnas[i-eliteCount]);
            }
        }

        private void Mutate(double[] childDna, double mutationRate)
        {
            for (int i = 0; i < childDna.Length; i++)
            {
                if (_randomProvider.NextDouble() < mutationRate)
                    childDna[i] += _randomProvider.NextDouble() - 0.5;
            }
        }

        private double[] Crossover(IDna parent1, IDna parent2)
        {
            var crossoverPoint = (int)Math.Floor(_randomProvider.NextDouble()*parent1.GetDna().Length);

            return parent1.GetDna().Take(crossoverPoint).Concat(parent2.GetDna().Skip(crossoverPoint)).ToArray();
        }

        private IFitDna RouletteWheelSelection(IFitDna[] fitDnas)
        {
            var fitnessSum = fitDnas.Sum(dna => dna.Fitness);
            var wheelHit = _randomProvider.NextDouble()*fitnessSum;

            var accumulatedFitness = 0d;

            foreach (var dna in fitDnas)
            {
                accumulatedFitness += dna.Fitness;

                if (wheelHit <= accumulatedFitness)
                    return dna;
            }

            return null;
        }
    }
}
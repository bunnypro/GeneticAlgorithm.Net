using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions;
using Bunnypro.GeneticAlgorithm.MultiObjective.Core;
using Bunnypro.GeneticAlgorithm.Primitives;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.NSGA2
{
    public class NSGA2<T> : DistinctMultiObjectiveGeneticOperation<T> where T : Enum
    {
        private readonly IMultiObjectiveGeneticOperation<T> _reproduction;
        private readonly IChromosomeEvaluator<T> _chromosomeEvaluator;
        private readonly IDistinctMultiObjectiveGeneticOperation<T> _offspringSelection;

        public NSGA2(IMultiObjectiveGeneticOperation<T> reproduction,
            IChromosomeEvaluator<T> chromosomeEvaluator,
            IReadOnlyDictionary<T, double> coefficients)
        {
            _reproduction = reproduction;
            _chromosomeEvaluator = chromosomeEvaluator;
            _offspringSelection = new OffspringSelection<T>(coefficients);
        }

        public override async Task<ImmutableHashSet<IChromosome<T>>> Operate(
            IEnumerable<IChromosome<T>> chromosomes,
            PopulationCapacity capacity,
            CancellationToken token = default)
        {
            var parents = chromosomes.ToArray();
            var offspring = (await _reproduction.Operate(parents, capacity, token)).ToList();
            offspring.AddRange(parents);
            await _chromosomeEvaluator.EvaluateAll(offspring, token);
            var allOffspring = await _offspringSelection.Operate(offspring.ToImmutableHashSet(), capacity, token);
            await _chromosomeEvaluator.EvaluateAll(allOffspring, token);
            return allOffspring;
        }
    }
}
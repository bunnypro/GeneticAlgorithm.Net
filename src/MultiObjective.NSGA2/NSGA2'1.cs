using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions;
using Bunnypro.GeneticAlgorithm.MultiObjective.Core;
using Bunnypro.GeneticAlgorithm.Primitives;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.NSGA2
{
    public class NSGA2<T> : MultiObjectiveGeneticOperation<T> where T : Enum
    {
        private readonly IMultiObjectiveGeneticOperation<T> _crossover;
        private readonly IMultiObjectiveGeneticOperation<T> _mutation;
        private readonly IObjectiveEvaluators<T> _objectiveValuesEvaluator;
        private readonly IMultiObjectiveGeneticOperation<T> _offspringSelection = new OffspringSelection<T>();

        public NSGA2(IMultiObjectiveGeneticOperation<T> crossover,
            IMultiObjectiveGeneticOperation<T> mutation,
            IObjectiveEvaluators<T> evaluators)
        {
            _crossover = crossover;
            _mutation = mutation;
            _objectiveValuesEvaluator = evaluators;
        }

        public override async Task<ImmutableHashSet<IChromosome<T>>> Operate(
            ImmutableHashSet<IChromosome<T>> chromosomes,
            PopulationCapacity capacity,
            CancellationToken token = default)
        {
            // TODO: Genetic Operations (Selection, Crossover, Mutation) <-- Parent Chromosomes with Evaluated Objective Values --> Offspring
            var offspring = await GenerateOffspring(chromosomes, capacity, token);

            // TODO: Evaluate Offspring Objective Values
            foreach (var child in offspring) child.ObjectiveValues = _objectiveValuesEvaluator.Evaluate(child);

            offspring.UnionWith(chromosomes);

            return await _offspringSelection.Operate(offspring.ToImmutableHashSet(), capacity, token);
        }

        private async Task<HashSet<IChromosome<T>>> GenerateOffspring(
            ImmutableHashSet<IChromosome<T>> chromosomes,
            PopulationCapacity capacity,
            CancellationToken token)
        {
            // TODO: Selection (Mating Pool and Selection)
            var offspring = new HashSet<IChromosome<T>>();

            // TODO: Crossover
            offspring.UnionWith(await _crossover.Operate(chromosomes, capacity, token));

            // TODO: Mutation
            offspring.UnionWith(await _mutation.Operate(chromosomes, capacity, token));

            return offspring;
        }
    }
}
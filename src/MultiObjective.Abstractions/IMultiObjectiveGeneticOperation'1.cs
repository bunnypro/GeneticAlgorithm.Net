using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Primitives;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions
{
    public interface IMultiObjectiveGeneticOperation<T> where T : Enum
    {
        Task<ImmutableHashSet<IChromosome<T>>> Operate(
            ImmutableHashSet<IChromosome<T>> chromosomes,
            PopulationCapacity capacity,
            CancellationToken token = default);
    }
}
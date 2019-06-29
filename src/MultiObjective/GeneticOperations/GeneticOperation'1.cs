using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions;
using Bunnypro.GeneticAlgorithm.Primitives;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.GeneticOperations
{
    public abstract class GeneticOperation<T> : IGeneticOperation where T : Enum
    {
        public async Task<ImmutableHashSet<IChromosome>> Operate(ImmutableHashSet<IChromosome> chromosomes, PopulationCapacity capacity, CancellationToken token = default)
        {
            var offspring = await Operate(chromosomes.Cast<IChromosome<T>>().ToImmutableHashSet(), capacity, token);
            return offspring.Cast<IChromosome>().ToImmutableHashSet();
        }

        public abstract Task<HashSet<IChromosome<T>>> Operate(ImmutableHashSet<IChromosome<T>> chromosomes, PopulationCapacity capacity, CancellationToken token);
    }
}
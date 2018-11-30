using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public abstract class ChromosomeFactory : IChromosomeFactory
    {
        public abstract IChromosome Create();

        public ImmutableHashSet<IChromosome> Create(int count)
        {
            var chromosomes = ImmutableHashSet.CreateBuilder<IChromosome>();
            while (chromosomes.Count < count) chromosomes.Add(Create());
            return chromosomes.ToImmutable();
        }
    }
}
using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public abstract class ChromosomeFactory : IChromosomeFactory
    {
        public abstract IChromosome Create();

        public HashSet<IChromosome> Create(int count)
        {
            var chromosomes = new HashSet<IChromosome>();

            while (chromosomes.Count < count)
            {
                chromosomes.Add(Create());
            }

            return chromosomes;
        }
    }
}

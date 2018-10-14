using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations.Generic
{
    public abstract class ChromosomeFactory<T> : IChromosomeFactory<T> where T : IChromosome
    {
        public abstract T Create();

        public HashSet<T> Create(int count)
        {
            var chromosomes = new HashSet<T>();

            while (chromosomes.Count < count)
            {
                chromosomes.Add(Create());
            }

            return chromosomes;
        }
    }
}

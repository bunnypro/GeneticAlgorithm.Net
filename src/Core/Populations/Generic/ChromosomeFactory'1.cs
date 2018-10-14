using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations.Generic
{
    public abstract class ChromosomeFactory<T> : IChromosomeFactory<T> where T : IChromosome
    {
        IChromosome IChromosomeFactory.Create()
        {
            return Create();
        }

        public abstract T Create();

        HashSet<IChromosome> IChromosomeFactory.Create(int count)
        {
            return new HashSet<IChromosome>(Create(count).Cast<IChromosome>());
        }

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

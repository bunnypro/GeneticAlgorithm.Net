using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperations
{
    public abstract class PreparableOperation<T> : GeneticOperation<T>, IPreparableOperation where T : IChromosome
    {
        public void Prepare(IEnumerable<IChromosome> initialChromosomes)
        {
            Prepare(initialChromosomes.Cast<T>());
        }

        protected abstract void Prepare(IEnumerable<T> initialChromosome);
    }
}
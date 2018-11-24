using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperators
{
    public abstract class PreparableOperator<T> : GeneticOperator<T>, IPreparableOperator where T : IChromosome
    {
        public void Prepare(IEnumerable<IChromosome> initialChromosomes)
        {
            Prepare(initialChromosomes.Cast<T>());
        }

        protected abstract void Prepare(IEnumerable<T> initialChromosome);
    }
}
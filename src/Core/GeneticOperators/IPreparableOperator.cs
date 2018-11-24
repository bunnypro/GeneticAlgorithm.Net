using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperators
{
    public interface IPreparableOperator : IGeneticOperator
    {
        void Prepare(IEnumerable<IChromosome> initialChromosomes);
    }
}
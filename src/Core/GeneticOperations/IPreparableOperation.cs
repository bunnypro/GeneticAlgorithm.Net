using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperations
{
    public interface IPreparableOperation : IGeneticOperation
    {
        void Prepare(IEnumerable<IChromosome> initialChromosomes);
    }
}
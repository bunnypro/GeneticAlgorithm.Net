using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Core.Chromosomes;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimpleChromosome : Chromosome
    {
        public SimpleChromosome(IEnumerable<object> genes) : base(genes)
        {
        }
    }
}
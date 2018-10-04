using System;
using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Core.Populations;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimpleChromosomeFactory : IChromosomeFactory<SimpleChromosome>
    {
        public SimpleChromosome Create()
        {
            return new SimpleChromosome(new List<object>
            {
                new Random().Next(100)
            });
        }

        public HashSet<SimpleChromosome> Create(int count)
        {
            var chromosomes = new HashSet<SimpleChromosome>();

            while (chromosomes.Count < count) chromosomes.Add(Create());

            return chromosomes;
        }
    }
}
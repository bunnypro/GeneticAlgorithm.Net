using System;
using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Chromosomes
{
    public class Chromosome : IChromosome
    {
        public Chromosome(IEnumerable<object> genes)
        {
            Genes = genes.ToList();
        }

        public IReadOnlyCollection<object> Genes { get; }
        public IComparable Fitness { get; set; }

        public bool Equals(IChromosome other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.GetType() == GetType() && Equals((Chromosome) other);
        }

        public sealed override bool Equals(object obj)
        {
            return Equals((IChromosome) obj);
        }

        protected virtual bool Equals(Chromosome other)
        {
            return !Genes.Except(other.Genes).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Genes != null && Genes.Count > 0 ?
                    Genes.Aggregate(0, (hashCode, gene) =>
                    {
                        if (hashCode == 0) return gene.GetHashCode();
                        return (hashCode * 397) ^ gene.GetHashCode();
                    }) :
                    0;
            }
        }
    }
}
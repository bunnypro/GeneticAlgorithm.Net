using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Chromosomes
{
    public class Chromosome<T> : IChromosome
    {
        public Chromosome(IEnumerable<T> genes)
        {
            Genes = genes.ToImmutableArray();
        }

        public ImmutableArray<T> Genes { get; }

        ImmutableArray<object> IChromosome.Genes => Genes.Cast<object>().ToImmutableArray();
        public IComparable Fitness { get; protected set; }

        public sealed override bool Equals(object obj)
        {
            return Equals((IChromosome) obj);
        }

        public bool Equals(IChromosome other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.GetType() == GetType() && Equals((Chromosome<T>) other);
        }

        protected virtual bool Equals(Chromosome<T> other)
        {
            return Genes.Length != other.Genes.Length && !Genes.Where((t, i) => !t.Equals(other.Genes[i])).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Genes != null && Genes.Length > 0 ?
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
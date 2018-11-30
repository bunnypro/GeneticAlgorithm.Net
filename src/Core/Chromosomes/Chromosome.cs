using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Chromosomes
{
    public class Chromosome : IChromosome, IEvaluableFitnessChromosome, IEnumerable<object>
    {
        public Chromosome(IEnumerable<object> genes)
        {
            Genes = genes.ToImmutableArray();
        }

        public ImmutableArray<object> Genes { get; }
        public IComparable Fitness { get; private set; }

        public virtual bool EvaluateFitness(IFitnessEvaluator evaluator)
        {
            Fitness = evaluator.Evaluate(this);
            return !ReferenceEquals(null, Fitness);
        }

        public sealed override bool Equals(object obj)
        {
            return Equals((IChromosome) obj);
        }

        public bool Equals(IChromosome other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.GetType() == GetType() && Equals((Chromosome) other);
        }

        protected virtual bool Equals(Chromosome other)
        {
            return !Genes.Except(other.Genes).Any();
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return ((IEnumerable<object>) Genes).GetEnumerator();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Core.GeneticOperations;
using Bunnypro.GeneticAlgorithm.MultiObjective.Fitness;
using Bunnypro.GeneticAlgorithm.MultiObjective.NonDominatedSorting;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.FrontsSelectionOperations
{
    public class ReferencePointsBasedSelectionOperation<TChromosome, TObjective> : GeneticOperation<TChromosome> where TChromosome : IMultiObjectiveFitnessChromosome<TObjective> where TObjective : Enum
    {
        private readonly INonDominatedSorting<TChromosome> _nonDominatedSorting;

        public ReferencePointsBasedSelectionOperation(INonDominatedSorting<TChromosome> nonDominatedSorting)
        {
            _nonDominatedSorting = nonDominatedSorting;
        }
        
        public override IEnumerable<TChromosome> Operate(IEnumerable<TChromosome> parents, int count)
        {
            var fronts = _nonDominatedSorting.Sort(parents);
            var selectedOffspring = new List<TChromosome>();
            ImmutableArray<TChromosome> lastSelectableFront;

            foreach (var front in fronts)
            {
                if (selectedOffspring.Count + front.Length > count)
                {
                    lastSelectableFront = front;
                    break;
                }

                selectedOffspring.AddRange(front);
                if (selectedOffspring.Count == count)
                    return selectedOffspring;
            }

            // TODO select by reference point
            // -> WIP Normalization: Find Ideal Point of current selected offspring, including from last selectable front.
            // -> TODO Distance Association: Find closest reference point of an offspring and calculate the distance.
            // -> TODO Niching(?)
            // -> TODO Choose remaining required offspring from last selectable front by Niche(?) value.

            var normalizableOffspring = lastSelectableFront.AddRange(selectedOffspring);
            var idealPoint = CalculateIdealPoint(normalizableOffspring);
            var offspringWithObjectivesDistance = CalculateObjectivesDistance(normalizableOffspring, idealPoint);

//            var remainingRequiredOffspringCount = count - selectedOffspring.Count;

            return selectedOffspring;
        }
        
        private IObjectiveValues<TObjective> CalculateIdealPoint(IEnumerable<TChromosome> chromosomes)
        {
            var objectives = Enum.GetValues(typeof(TObjective)).Cast<TObjective>();
            var idealPoint = chromosomes.Aggregate(new ObjectiveValues<TObjective>(), (point, chromosome) =>
            {
                foreach (var objective in objectives)
                    if (!point.ContainsKey(objective) || chromosome.Fitness[objective].CompareTo(point[objective]) > 0)
                        point[objective] = chromosome.Fitness[objective];

                return point;
            });
            return idealPoint;
        }
        
        private IEnumerable<KeyValuePair<TChromosome, ObjectiveValues<TObjective>>> CalculateObjectivesDistance(IEnumerable<TChromosome> chromosomes, IObjectiveValues<TObjective> idealPoint)
        {
            var objectives = Enum.GetValues(typeof(TObjective)).Cast<TObjective>().ToArray();
            foreach (var chromosome in chromosomes)
            {
                var objectivesDistance = new ObjectiveValues<TObjective>();
                foreach (var objective in objectives)
                {
                    objectivesDistance[objective] = Math.Abs(idealPoint[objective].CompareTo(chromosome.Fitness[objective]));
                }

                yield return new KeyValuePair<TChromosome, ObjectiveValues<TObjective>>(chromosome, objectivesDistance);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Core.GeneticOperations;
using Bunnypro.GeneticAlgorithm.MultiObjective.NonDominatedSorting;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.FrontsSelectionOperations
{
    public class ElitismFrontSelectionOperation<T> : GeneticOperation<T> where T : IChromosome
    {
        private readonly INonDominatedSorting<T> _nonDominatedSorting;

        public ElitismFrontSelectionOperation(INonDominatedSorting<T> nonDominatedSorting)
        {
            _nonDominatedSorting = nonDominatedSorting;
        }

        public override IEnumerable<T> Operate(IEnumerable<T> parents, int count)
        {
            var fronts = _nonDominatedSorting.Sort(parents);
            
            var selectedOffspring = new List<T>();
            foreach (var front in fronts)
            {
                var selectedCount = selectedOffspring.Count;
                var remainingRequiredOffspringCount = count - selectedCount;
                var isCurrentFrontAdditionMakesOverflow = front.Length > remainingRequiredOffspringCount;
                var selectedOffspringFromCurrentFront = isCurrentFrontAdditionMakesOverflow ?
                    front.Take(count - selectedCount) :
                    front;
                selectedOffspring.AddRange(selectedOffspringFromCurrentFront);
                if (selectedOffspring.Count >= count)
                    break;
            }
            return selectedOffspring;
        }
    }
}
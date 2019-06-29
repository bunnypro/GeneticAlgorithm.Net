using System;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.MultiObjective.Primitives;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions
{
    public interface IChromosome<T> : IChromosome where T : Enum
    {
        ObjectiveValues<T> ObjectiveValues { get; set; }
    }
}
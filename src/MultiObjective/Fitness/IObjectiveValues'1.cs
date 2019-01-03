using System;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Fitness
{
    public interface IObjectiveValues<in T> : IComparable where T : Enum
    {
        IComparable this[T objective] { get; set; }
    }
}
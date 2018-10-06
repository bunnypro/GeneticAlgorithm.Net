namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface ITerminationCondition
    {
        bool Fulfilled(IEvolutionState state);
    }
}
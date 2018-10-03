namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface ITerminationCondition
    {
        void Start();
        void Pause();
        void Reset();
        
        bool Fulfilled();
    }
}
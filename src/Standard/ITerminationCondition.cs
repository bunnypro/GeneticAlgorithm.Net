namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface ITerminationCondition
    {
        bool Fulfilled { get; }

        void Start();
        void Pause();
        void Reset();
    }
}

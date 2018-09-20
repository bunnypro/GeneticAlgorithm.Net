namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface ITermination
    {
        void Start();
        void Pause();
        void Reset();
        
        bool Fulfilled();
    }
}
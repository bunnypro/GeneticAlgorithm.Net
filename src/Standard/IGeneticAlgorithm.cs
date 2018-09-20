using System;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IGeneticAlgorithm
    {
        int GenerationNumber { get; }
        bool Evolving { get; }
        
        Task Evolve();
        Task EvolveUntil(Func<bool> fulfilled);
        Task EvolveUntil(ITermination termination);
        Task Stop();
        Task Reset();
    }
}

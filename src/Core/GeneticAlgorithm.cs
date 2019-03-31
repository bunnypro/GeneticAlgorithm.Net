using System.Threading;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticAlgorithm
    {
        private readonly GeneticAlgorithmStates _states;
        private readonly IPopulation _population;
        private readonly IGeneticOperation _strategy;
        private readonly Mutex _evolution = new Mutex();

        public GeneticAlgorithm(IPopulation population, IGeneticOperation strategy)
        {
            _states = new GeneticAlgorithmStates();
            _population = population;
            _strategy = strategy;
        }

        public IGeneticAlgorithmStates States => _states;
        public IReadOnlyPopulation Population => _population;

        public async Task EvolveOnce(CancellationToken token = default)
        {
            _evolution.WaitOne();
            {
                if (!_population.IsInitialized) _population.Initialize();
                var offspring = await _strategy.Operate(_population.Chromosomes, token);
                _population.RegisterOffspring(offspring);
                _states.EvolutionCount++;
            }
            _evolution.ReleaseMutex();
        }

        private class GeneticAlgorithmStates : IGeneticAlgorithmStates
        {
            public int EvolutionCount { get; set; }
        }
    }
}
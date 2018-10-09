using System;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Core.Exceptions;
using Bunnypro.GeneticAlgorithm.Core.Terminations;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticAlgorithm : IGeneticAlgorithm
    {
        private readonly object _evolutionPreparation = new object();
        private bool _evolutionCanceled;
        
        private readonly IPopulation _population;
        private readonly IEvolutionStrategy _strategy;
        private EvolutionState _state;

        public GeneticAlgorithm(IPopulation population, IEvolutionStrategy strategy)
        {
            _population = population;
            _strategy = strategy;
            _state = new EvolutionState();
        }

        public ITerminationCondition TerminationCondition { get; set; }

        public IEvolutionState State => _state;
        public IReadOnlyPopulation Population => _population;

        public async Task Evolve(Func<IEvolutionState, bool> terminationCondition)
        {
            await Evolve(new FunctionTerminationCondition(terminationCondition));
        }

        public async Task Evolve(ITerminationCondition terminationCondition)
        {
            lock (_evolutionPreparation)
            {
                if (_state.Evolving) throw new EvolutionRunningException();

                _evolutionCanceled = false;
                TerminationCondition = terminationCondition;

                if (State.EvolutionNumber == 0)
                {
                    _state.Reset();
                    _population.Initialize();
                    _strategy.Prepare(_population.Chromosomes);
                }
                else if (TerminationCondition.Fulfilled(State))
                {
                    return;
                }

                _state.Evolving = true;
            }

            await Task.Run(() =>
            {
                do
                {
                    var startTime = DateTime.Now;
                    var offspring = _strategy.GenerateOffspring(_population.Chromosomes);
                    _state.EvolutionTime += DateTime.Now - startTime;
                    _state.EvolutionNumber++;

                    _population.StoreOffspring(offspring);
                } while (!(_evolutionCanceled || TerminationCondition.Fulfilled(State)));
            });

            _state.Evolving = false;
        }

        public async Task Evolve()
        {
            await Evolve(TerminationCondition ?? new FunctionTerminationCondition(state => false));
        }

        public void Stop()
        {
            if (!_state.Evolving) return;

            _evolutionCanceled = true;
        }

        public void Reset()
        {
            if (_state.Evolving) throw new EvolutionRunningException();

            _state.Reset();
        }

        public bool TryReset()
        {
            try
            {
                Reset();
            }
            catch (EvolutionRunningException)
            {
                return false;
            }

            return true;
        }

        private struct EvolutionState : IEvolutionState
        {
            public int EvolutionNumber { get; set; }
            public TimeSpan EvolutionTime { get; set; }
            public bool Evolving { get; set; }

            public void Reset()
            {
                EvolutionNumber = 0;
                EvolutionTime = TimeSpan.Zero;
                Evolving = false;
            }
        }
    }
}
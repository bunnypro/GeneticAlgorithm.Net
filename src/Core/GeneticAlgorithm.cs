using System;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Core.Exceptions;
using Bunnypro.GeneticAlgorithm.Core.Populations;
using Bunnypro.GeneticAlgorithm.Core.Strategies;
using Bunnypro.GeneticAlgorithm.Core.Terminations;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticAlgorithm : IGeneticAlgorithm
    {
        private readonly object _evolution = new object();
        private bool _evolutionCanceled;

        private ITerminationCondition _terminationCondition;

        private readonly IEvolvablePopulation _population;
        private readonly IEvolutionStrategy _strategy;
        private EvolutionState _state;

        public GeneticAlgorithm(IEvolvablePopulation population, IEvolutionStrategy strategy)
        {
            _population = population;
            _strategy = strategy;
            _state = new EvolutionState();
        }

        public IEvolutionState State => _state;
        public IPopulation Population => _population;

        public async Task Evolve()
        {
            await Evolve(_terminationCondition ?? new FunctionTerminationCondition(state => false));
        }

        public async Task Evolve(Func<IEvolutionState, bool> terminationCondition)
        {
            await Evolve(new FunctionTerminationCondition(terminationCondition));
        }

        public async Task Evolve(ITerminationCondition terminationCondition)
        {
            lock (_evolution)
            {
                if (_state.Evolving) throw new EvolutionLockedException();

                _evolutionCanceled = false;
                _terminationCondition = terminationCondition;

                if (State.EvolutionNumber == 0)
                {
                    _state.Reset();

                    // still confused about
                    // 1. Which is responsible for population initialization
                    // 2. Does evolution strategy really need after initialization hook
                    // 3. Does evolution strategy really need initial chromosomes
                    _population.Initialize();
                    _strategy.Prepare(_population.InitialChromosomes);
                }
                else if (_terminationCondition.Fulfilled(State))
                {
                    return;
                }

                _state.Evolving = true;
            }

            await Task.Run(() =>
            {
                lock (_population)
                {
                    do
                    {
                        var startTime = DateTime.Now;
                        var offspring = _strategy.GenerateOffspring(_population.Chromosomes);
                        _state.EvolutionTime += DateTime.Now - startTime;
                        _state.EvolutionNumber++;

                        _population.StoreOffspring(offspring);
                    } while (!(_evolutionCanceled || _terminationCondition.Fulfilled(State)));
                }
            });

            _state.Evolving = false;
        }

        public void Stop()
        {
            lock (_evolution)
                if (_state.Evolving)
                    _evolutionCanceled = true;
        }

        public void Reset()
        {
            lock (_evolution)
            {
                if (_state.Evolving) throw new EvolutionLockedException();
                _state.Reset();
            }
        }

        public bool TryReset()
        {
            try
            {
                Reset();
            }
            catch (EvolutionLockedException)
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
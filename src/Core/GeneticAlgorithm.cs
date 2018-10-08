using System;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Core.Exceptions;
using Bunnypro.GeneticAlgorithm.Core.Terminations;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticAlgorithm : IGeneticAlgorithm
    {
        private readonly object _evolutionPreparation = new object();
        private EvolutionState _state;
        private bool _evolutionCanceled;

        public GeneticAlgorithm(IPopulation population, IEvolutionStrategy strategy)
        {
            Population = population;
            Strategy = strategy;
            _state = new EvolutionState();
        }

        public IEvolutionState State => _state;
        public IPopulation Population { get; }
        public IEvolutionStrategy Strategy { get; }

        public ITerminationCondition TerminationCondition { get; set; }

        public async Task Evolve()
        {
            await Evolve(TerminationCondition ?? new FunctionTerminationCondition(state => false));
        }

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
                    Population.Initialize();
                    Strategy.Prepare(Population.Chromosomes);
                }
                else if (TerminationCondition.Fulfilled(State)) return;

                _state.Evolving = true;
            }

            await Task.Run(() =>
            {
                do
                {
                    var startTime = DateTime.Now;
                    var offspring = Strategy.GenerateOffspring(Population.Chromosomes);
                    _state.EvolutionTime += DateTime.Now - startTime;
                    _state.EvolutionNumber++;

                    Population.StoreOffspring(offspring);
                } while (!(_evolutionCanceled || TerminationCondition.Fulfilled(State)));
            });

            _state.Evolving = false;
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
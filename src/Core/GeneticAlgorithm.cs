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
        private CancellationTokenSource _evolutionCts;
        private EvolutionState _state;

        public GeneticAlgorithm(IPopulation population, IEvolutionStrategy evolutionStrategy)
        {
            Population = population;
            EvolutionStrategy = evolutionStrategy;
            _state = new EvolutionState();
        }

        public IPopulation Population { get; }
        public IEvolutionStrategy EvolutionStrategy { get; }

        public ITerminationCondition TerminationCondition { get; set; }

        public IEvolutionState State => _state;

        public async Task Evolve()
        {
            await EvolveUntil(TerminationCondition ?? new FunctionTerminationCondition(state => false));
        }

        public async Task EvolveUntil(Func<IEvolutionState, bool> fulfilled)
        {
            await EvolveUntil(new FunctionTerminationCondition(fulfilled));
        }

        public async Task EvolveUntil(ITerminationCondition terminationCondition)
        {
            lock (_evolutionPreparation)
            {
                if (_state.Evolving) throw new EvolutionRunningException();

                TerminationCondition = terminationCondition;

                if (State.EvolutionNumber == 0)
                {
                    Prepare();
                    Population.Initialize();
                    EvolutionStrategy.Prepare(Population.Chromosomes);
                }

                if (TerminationCondition.Fulfilled(State)) return;

                _state.Evolving = true;
            }

            using (_evolutionCts = new CancellationTokenSource())
            {
                await Task.Factory.StartNew(() =>
                {
                    do
                    {
                        var startTime = DateTime.Now;
                        var offspring = EvolutionStrategy.GenerateOffspring(Population.Chromosomes);
                        _state.EvolutionTime += DateTime.Now - startTime;
                        _state.EvolutionNumber++;
                        
                        Population.StoreOffspring(offspring);
                    } while (!(_evolutionCts.Token.IsCancellationRequested || TerminationCondition.Fulfilled(State)));
                }, _evolutionCts.Token);

                _state.Evolving = false;
            }
        }

        public void Stop()
        {
            if (!_state.Evolving) return;
            
            _evolutionCts.Cancel();
        }

        public void Reset()
        {
            if (_state.Evolving) throw new EvolutionRunningException();

            Prepare();
        }

        private void Prepare()
        {
            _evolutionCts = null;
            _state.Reset();
            Population.Reset();
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
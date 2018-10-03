using System;
using System.Timers;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Termination
{
    public class TimeLimitTerminationCondition : ITerminationCondition
    {
        private readonly double _timeLimit;
        private Timer _timer;
        private bool _disposed;

        public bool Fulfilled { get; private set; }

        public TimeLimitTerminationCondition(TimeSpan timeSpan) : this(timeSpan.TotalMilliseconds)
        {
        }

        public TimeLimitTerminationCondition(double timeLimit)
        {
            _timeLimit = timeLimit;
            Prepare();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public void Reset()
        {
            if (!_disposed)
            {
                _timer.Dispose();
            }

            Prepare();
        }

        private void Prepare()
        {
            Fulfilled = false;
            _disposed = false;
            _timer = new Timer(_timeLimit);
            _timer.Disposed += (o, e) => { _disposed = true; };
            _timer.Elapsed += (s, o) =>
            {
                _timer.Stop();
                _timer.Dispose();
                Fulfilled = true;
            };
        }
    }
}

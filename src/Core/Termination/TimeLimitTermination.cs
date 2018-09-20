using System;
using System.Threading.Tasks;
using System.Timers;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Termination
{
    public class TimeLimitTermination : ITermination
    {
        private readonly double _interval;
        private Timer _timer;
        private bool _intervalReached;

        public TimeLimitTermination(double interval)
        {
            _interval = interval;
            Prepare();
        }

        public TimeLimitTermination(TimeSpan timeSpan) : this(timeSpan.TotalMilliseconds)
        {
        }

        public bool Fulfilled()
        {
            return _intervalReached;
        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        public void Pause()
        {
            _timer.Enabled = false;
        }

        public void Reset()
        {
            _timer.Dispose();
            Prepare();
        }

        private void Prepare()
        {
            _intervalReached = false;
            _timer = new Timer(_interval);
            _timer.Elapsed += (s, o) =>
            {
                _timer.Stop();
                _timer.Dispose();
                _intervalReached = true;
            };
        }
    }
}
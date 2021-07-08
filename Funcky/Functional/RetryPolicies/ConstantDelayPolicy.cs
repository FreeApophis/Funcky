using System;

namespace Funcky
{
    public class ConstantDelayPolicy : IRetryPolicy
    {
        private readonly TimeSpan _delay;

        public ConstantDelayPolicy(int maxRetry, TimeSpan delay)
            => (MaxRetries, _delay) = (maxRetry, delay);

        public int MaxRetries { get; }

        public TimeSpan Duration(int onRetryCount) => _delay;
    }
}

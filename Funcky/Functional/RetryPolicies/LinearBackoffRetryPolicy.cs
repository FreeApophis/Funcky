using System;

namespace Funcky
{
    public sealed class LinearBackoffRetryPolicy : IRetryPolicy
    {
        private readonly TimeSpan _firstDelay;

        public LinearBackoffRetryPolicy(int maxRetry, TimeSpan firstDelay)
            => (MaxRetries, _firstDelay) = (maxRetry, firstDelay);

        public int MaxRetries { get; }

        public TimeSpan Duration(int onRetryCount) => _firstDelay.Multiply(onRetryCount);
    }
}

using System;

namespace Funcky.Monads
{
    public static partial class StateExtensions
    {
        public static State<TState, TResult> SelectMany<TState, TSource, TSelector, TResult>(
            this State<TState, TSource> source,
            Func<TSource, State<TState, TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector)
            => oldState
                =>
                {
                    var (value, state) = source(oldState);
                    var (selector1, newState) = selector(value)(state);

                    return (resultSelector(value, selector1), newState);
                };

        public static State<TState, TResult> Select<TState, TSource, TResult>(
            this State<TState, TSource> source,
            Func<TSource, TResult> selector)
            => oldState
                =>
                {
                    var (value, newState) = source(oldState);
                    return (selector(value), newState);
                };

        public static State<TState, TSource> State<TState, TSource>(this TSource value)
            => oldState
                => (value, oldState);
    }
}

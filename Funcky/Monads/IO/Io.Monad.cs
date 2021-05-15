using System;

namespace Funcky.Monads
{
    public static partial class IoExtensions
    {
        public static Io<TResult> SelectMany<TSource, TSelector, TResult>(
            this Io<TSource> source,
            Func<TSource, Io<TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector)
            => ()
                => ResultSelector(resultSelector, selector, source());

        public static Io<TResult> Select<TSource, TResult>(
            this Io<TSource> source,
            Func<TSource, TResult> selector)
            => source.SelectMany(value => selector(value).Io(), (value, result) => result);

        public static Io<TSource> Io<TSource>(this TSource value)
            => ()
                => value;

        private static TResult ResultSelector<TSource, TSelector, TResult>(
            Func<TSource, TSelector, TResult> resultSelector,
            Func<TSource, Io<TSelector>> selector,
            TSource value)
            => resultSelector(value, selector(value)());
    }
}

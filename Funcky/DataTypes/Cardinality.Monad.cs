using System;

namespace Funcky.DataTypes
{
    public readonly partial struct Cardinality<TItem>
    {
        public Cardinality<TResult> Select<TResult>(Func<TItem, TResult> selector)
            where TResult : notnull
            => Match(
                zero: Cardinality<TResult>.Zero,
                one: item => Cardinality.One(selector(item)),
                many: Cardinality<TResult>.Many);

        public Cardinality<TResult> SelectMany<TResult>(
            Func<TItem, Cardinality<TResult>> selector)
            where TResult : notnull
            => SelectMany(selector, (_, result) => result);

        public Cardinality<TResult> SelectMany<TSelector, TResult>(
            Func<TItem, Cardinality<TSelector>> selector,
            Func<TItem, TSelector, TResult> resultSelector)
            where TSelector : notnull
            where TResult : notnull
            => Match(
                zero: Cardinality<TResult>.Zero,
                one: item => selector(item).Match(
                    zero: Cardinality<TResult>.Zero,
                    one: selected => Cardinality.One(resultSelector(item, selected)),
                    many: Cardinality<TResult>.Many),
                many: Cardinality<TResult>.Many);
    }
}

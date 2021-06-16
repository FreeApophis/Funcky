using System;
using System.Diagnostics.Contracts;
using Funcky.Monads;

namespace Funcky.DataTypes
{
    public readonly partial struct Cardinality<TItem>
        where TItem : notnull
    {
        private readonly Discriminator _discriminator;
        private readonly TItem _value;

        internal Cardinality(TItem value)
            : this(Discriminator.One, value)
        {
        }

        private Cardinality(Discriminator discriminator, TItem value = default!)
        {
            _discriminator = discriminator;
            _value = value;
        }

        private enum Discriminator : byte
        {
            Zero,
            One,
            Many,
        }

        public static Cardinality<TItem> Zero => default;

        public static Cardinality<TItem> Many => new(Discriminator.Many);

        [Pure]
        public TResult Match<TResult>(
            Func<TResult> zero,
            Func<TItem, TResult> one,
            Func<TResult> many)
            => _discriminator switch
            {
                Discriminator.Zero => zero(),
                Discriminator.One => one(_value),
                Discriminator.Many => many(),
                _ => throw new InvalidOperationException("unreachable"),
            };

        [Pure]
        public TResult Match<TResult>(
            TResult zero,
            Func<TItem, TResult> one,
            TResult many)
            => _discriminator switch
            {
                Discriminator.Zero => zero,
                Discriminator.One => one(_value),
                Discriminator.Many => many,
                _ => throw new InvalidOperationException("unreachable"),
            };

        public Option<TItem> ToOption(Func<Option<TItem>> many)
            => Match(zero: Option<TItem>.None, one: Option.Some, many: many);

        public Option<TItem> ToOption(Func<TItem> many)
            => ToOption(() => Option.Some(many()));

        public override string ToString()
            => Match(
                zero: () => "Zero",
                one: item => $"One({item})",
                many: () => "Many");
    }

    public static class Cardinality
    {
        [Pure]
        public static Cardinality<TItem> One<TItem>(TItem item)
            where TItem : notnull => new(item);
    }
}

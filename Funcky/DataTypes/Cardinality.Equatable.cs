using System;
using System.Collections.Generic;

namespace Funcky.DataTypes
{
    public readonly partial struct Cardinality<TItem> : IEquatable<Cardinality<TItem>>
    {
        public static bool operator ==(Cardinality<TItem> left, Cardinality<TItem> right) => left.Equals(right);

        public static bool operator !=(Cardinality<TItem> left, Cardinality<TItem> right) => !left.Equals(right);

        public bool Equals(Cardinality<TItem> other)
            => _discriminator == other._discriminator && EqualityComparer<TItem>.Default.Equals(_value, other._value);

        public override bool Equals(object? obj) => obj is Cardinality<TItem> other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(_discriminator, _value);
    }
}

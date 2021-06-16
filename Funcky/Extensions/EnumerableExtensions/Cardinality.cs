using System.Collections.Generic;
using Funcky.DataTypes;

namespace Funcky.Extensions
{
    public static partial class EnumerableExtensions
    {
        public static Cardinality<TSource> Cardinality<TSource>(this IEnumerable<TSource> source)
            where TSource : notnull
        {
            using var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                return DataTypes.Cardinality<TSource>.Zero;
            }

            var value = enumerator.Current;
            return enumerator.MoveNext()
                ? DataTypes.Cardinality<TSource>.Many
                : DataTypes.Cardinality.One(value);
        }
    }
}

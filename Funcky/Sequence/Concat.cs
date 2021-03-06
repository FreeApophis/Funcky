using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using static Funcky.Functional;

namespace Funcky
{
    public static partial class Sequence
    {
        [Pure]
        public static IEnumerable<TSource> Concat<TSource>(params IEnumerable<TSource>[] sources) => Concat(sources.AsEnumerable());

        [Pure]
        public static IEnumerable<TSource> Concat<TSource>(IEnumerable<IEnumerable<TSource>> sources) => sources.SelectMany(Identity);
    }
}

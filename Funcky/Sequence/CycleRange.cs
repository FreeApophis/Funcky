using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Funcky.Extensions;
using static Funcky.Functional;

namespace Funcky
{
    public static partial class Sequence
    {
        /// <summary>
        /// Generates a sequence that contains the same sequence of elements over and over again as an endless generator.
        /// </summary>
        /// <typeparam name="TItem">Type of the elements to be cycled.</typeparam>
        /// <param name="sequence">The sequence of elements which are cycled. Throws an exception if the sequence is empty.</param>
        /// <returns>Returns an infinite IEnumerable repeating the same sequence of elements.</returns>
        [Pure]
        public static IEnumerable<TItem> CycleRange<TItem>(IEnumerable<TItem> sequence)
            where TItem : notnull
        {
            var list = sequence.Materialize();

            if (list.None())
            {
                throw new ArgumentException("An empty sequence cannot be cycled", nameof(sequence));
            }

            return CycleRangeExpression(list);
        }

        private static IEnumerable<TItem> CycleRangeExpression<TItem>(IEnumerable<TItem> sequence)
            where TItem : notnull
            => Generate(Unit.Value, Identity)
                .SelectMany(_ => sequence);
    }
}

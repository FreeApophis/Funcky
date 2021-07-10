using System;
using System.Linq;
using Funcky.Extensions;
using Funcky.Monads;
using Funcky.Test.TestUtils;
using Xunit;

namespace Funcky.Test.Extensions.QueryableExtensions
{
    public sealed class PreventAccidentalUseAsEnumerableTest
    {
        [Fact]
        public void ThrowsWhenQueryableIsConvertedToEnumerableAndEnumerated()
            => Assert.Throws<InvalidOperationException>(() =>
                Enumerable.Empty<int>()
                    .AsQueryable()
                    .PreventAccidentalUseAsEnumerable()
                    .AsEnumerable()
                    .Materialize());

        [Fact]
        public void ThrowsWhenOperatorsAreAppliedToQueryableThenConvertedToEnumerableAndEnumerated()
            => Assert.Throws<InvalidOperationException>(() =>
                Enumerable.Empty<int>()
                    .AsQueryable()
                    .PreventAccidentalUseAsEnumerable()
                    .Select(x => x)
                    .Where(_ => true)
                    .Distinct()
                    .AsEnumerable()
                    .Materialize());

        [Fact]
        public void ThrowsWhenQueryableIsConvertedToEnumerableUsingOperatorOnlyAvailableOnEnumerable()
            => Assert.Throws<InvalidOperationException>(() =>
                Enumerable.Empty<int>()
                    .AsQueryable()
                    .PreventAccidentalUseAsEnumerable()
                    .Materialize());

        [Fact]
        public void ThrowsWhenQueryableIsConvertedToEnumerableUsingOperatorOverloadOnlyAvailableOnEnumerable()
            => Assert.Throws<InvalidOperationException>(() =>
                Enumerable.Empty<int>()
                    .AsQueryable()
                    .PreventAccidentalUseAsEnumerable()
                    .Select(Option.Some)
                    .Materialize());

        [Fact]
        public void DoesNotThrowWhenMaterializedUsingQueryableOperator()
            => _ = Enumerable.Empty<int>()
                .AsQueryable()
                .PreventAccidentalUseAsEnumerable()
                .FirstOrDefault();
    }
}

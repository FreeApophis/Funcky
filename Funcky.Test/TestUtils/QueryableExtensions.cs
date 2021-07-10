using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Funcky.Test.TestUtils
{
    internal static class QueryableExtensions
    {
        public static IQueryable<TSource> PreventAccidentalUseAsEnumerable<TSource>(this IQueryable<TSource> source)
            => new QueryableDisallowingUseAsEnumerable<TSource>(source);

        private sealed class QueryableDisallowingUseAsEnumerable<TSource> : IQueryable<TSource>
        {
            private readonly IQueryable<TSource> _queryable;

            public QueryableDisallowingUseAsEnumerable(IQueryable<TSource> queryable) => _queryable = queryable;

            public Type ElementType => _queryable.ElementType;

            public Expression Expression => _queryable.Expression;

            public IQueryProvider Provider => new QueryProvider(_queryable.Provider);

            public IEnumerator<TSource> GetEnumerator() => throw new InvalidOperationException("Queryable should not be used as Enumerable");

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private sealed class QueryProvider : IQueryProvider
            {
                private readonly IQueryProvider _provider;

                public QueryProvider(IQueryProvider provider) => _provider = provider;

                public IQueryable CreateQuery(Expression expression) => throw new NotImplementedException();

                public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
                    => new QueryableDisallowingUseAsEnumerable<TElement>(_provider.CreateQuery<TElement>(expression));

                public object? Execute(Expression expression) => throw new NotImplementedException();

                public TResult Execute<TResult>(Expression expression)
                    => _provider.Execute<TResult>(expression);
            }
        }
    }
}

using System.Linq;
using Funcky.Extensions;
using Funcky.Test.TestUtils;
using Xunit;

namespace Funcky.Test.Extensions.QueryableExtensions
{
    public sealed class SingleOrNoneTest
    {
        [Fact]
        public void SingleOrNoneIsEvaluatedUsingExpressions()
            => _ = Enumerable.Empty<int>()
                .AsQueryable()
                .PreventAccidentalUseAsEnumerable()
                .SingleOrNone();
    }
}

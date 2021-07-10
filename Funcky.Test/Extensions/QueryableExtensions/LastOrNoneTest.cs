using System.Linq;
using Funcky.Extensions;
using Funcky.Test.TestUtils;
using Xunit;

namespace Funcky.Test.Extensions.QueryableExtensions
{
    public sealed class LastOrNoneTest
    {
        [Fact]
        public void LastOrNoneIsEvaluatedUsingExpressions()
            => _ = Enumerable.Empty<int>()
                .AsQueryable()
                .PreventAccidentalUseAsEnumerable()
                .LastOrNone();
    }
}

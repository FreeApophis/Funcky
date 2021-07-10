using System.Linq;
using Funcky.Extensions;
using Funcky.Test.TestUtils;
using Xunit;

namespace Funcky.Test.Extensions.QueryableExtensions
{
    public sealed class FirstOrNoneTest
    {
        [Fact]
        public void FirstOrNoneIsEvaluatedUsingExpressions()
            => _ = Enumerable.Empty<int>()
                .AsQueryable()
                .PreventAccidentalUseAsEnumerable()
                .FirstOrNone();
    }
}

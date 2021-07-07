using System.Threading.Tasks;
using Xunit;
using VerifyCS = Funcky.Analyzer.Test.CSharpCodeFixVerifier<Funcky.Analyzer.EnumerableRepeatNeverAnalyzer, Funcky.Analyzer.EnumerableRepeatNeverCodeFix>;

namespace Funcky.Analyzer.Test
{
    public class EnumerableRepeatNeverTest
    {
        [Fact]
        public async Task EnumerableRepeatWithAnyNumberButZeroIssuesNoDiagnostic()
        {
            var test = @"
    using System;
    using System.Linq;
    
    namespace ConsoleApplication1
    {
        class Program
        {
            private void Syntax()
            {
                var single = Enumerable.Repeat(1337, 1);
            }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [Fact]
        public async Task UsingEnumerableRepeatNeverShowsTheSequenceReturnDiagnostic()
        {
            var test = @"
    using System;
    using System.Linq;

    namespace ConsoleApplication1
    {
        class Program
        {
            private void Syntax()
            {
                var single = Enumerable.Repeat(""Hello world!"", 0);
            }
        }
    }";

            var fixtest = @"
    using System;
    using System.Linq;

    namespace ConsoleApplication1
    {
        class Program
        {
            private void Syntax()
            {
                var single = Enumerable.Empty<string>();
            }
        }
    }";
            var expected = VerifyCS.Diagnostic(nameof(EnumerableRepeatNeverAnalyzer)).WithSpan(11, 30, 11, 66).WithArguments("\"Hello world!\"", "string");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }

        [Fact]
        public async Task UsingEnumerableRepeatNeverWorksWithDifferentTypes()
        {
            var test = @"
    using System;
    using System.Linq;

    namespace ConsoleApplication1
    {
        class Program
        {
            private void Syntax()
            {
                var single = Enumerable.Repeat(1337, 0);
            }
        }
    }";

            var fixtest = @"
    using System;
    using System.Linq;

    namespace ConsoleApplication1
    {
        class Program
        {
            private void Syntax()
            {
                var single = Enumerable.Empty<int>();
            }
        }
    }";
            var expected = VerifyCS.Diagnostic(nameof(EnumerableRepeatNeverAnalyzer)).WithSpan(11, 30, 11, 56).WithArguments("1337", "int");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}

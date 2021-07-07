using System.Threading.Tasks;
using Xunit;
using VerifyCS = Funcky.Analyzer.Test.CSharpCodeFixVerifier<Funcky.Analyzer.EnumerableRepeatOnceAnalyzer, Funcky.Analyzer.EnumerableRepeatOnceCodeFix>;

namespace Funcky.Analyzer.Test
{
    public class EnumerableRepeatOnceTest
    {
        [Fact]
        public async Task EnumerableRepeatWithAnyNumberButOneIssuesNoDiagnostic()
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
                var single = Enumerable.Repeat(1337, 2);
            }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [Fact]
        public async Task UsingEnumerableRepeatOnceShowsTheSequenceReturnDiagnostic()
        {
            var test = @"
    using System;
    using System.Linq;

    namespace ConsoleApplication1
    {
        class Sequence {
            public static string Return(string value) => value;           
        }
        class Program
        {
            private void Syntax()
            {
                var single = Enumerable.Repeat(""Hello world!"", 1);
            }
        }
    }";

            var fixtest = @"
    using System;
    using System.Linq;

    namespace ConsoleApplication1
    {
        class Sequence {
            public static string Return(string value) => value;           
        }
        class Program
        {
            private void Syntax()
            {
                var single = Sequence.Return(""Hello world!"");
            }
        }
    }";
            var expected = VerifyCS.Diagnostic(nameof(EnumerableRepeatOnceAnalyzer)).WithSpan(14, 30, 14, 66).WithArguments("\"Hello world!\"");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Funcky.Analyzer.LocalizedResourceLoader;
using static Funcky.Analyzer.Resources;

namespace Funcky.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EnumerableRepeatOnceAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = nameof(EnumerableRepeatOnceAnalyzer);
        private const string Category = nameof(Funcky);

        private static readonly LocalizableString Title = LoadFromResource(nameof(EnumerableRepeatOnceAnalyzerTitle));
        private static readonly LocalizableString MessageFormat = LoadFromResource(nameof(EnumerableRepeatOnceAnalyzerMessageFormat));
        private static readonly LocalizableString Description = LoadFromResource(nameof(EnumerableRepeatOnceAnalyzerDescription));

        private static readonly DiagnosticDescriptor Rule = new(nameof(EnumerableRepeatOnceAnalyzer), Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(FindEnumerableRepeateOnce, SyntaxKind.InvocationExpression);
        }

        private void FindEnumerableRepeateOnce(SyntaxNodeAnalysisContext context)
        {
            if (IsRepeatOnce(new SyntaxMatcher(context)))
            {
                context.ReportDiagnostic(CreateDiagnostic(context));
            }
        }

        private static bool IsRepeatOnce(SyntaxMatcher syntax)
            => syntax.MatchStaticCall(nameof(Enumerable), nameof(Enumerable.Repeat))
                && syntax.MatchArgument(Argument.Second, 1);

        private static Diagnostic CreateDiagnostic(SyntaxNodeAnalysisContext context)
            => CreateDiagnostic(new SyntaxMatcher(context), (InvocationExpressionSyntax)context.Node);

        private static Diagnostic CreateDiagnostic(SyntaxMatcher syntax, InvocationExpressionSyntax invocationExpr)
            => Diagnostic.Create(Rule, invocationExpr.GetLocation(), syntax.GetArgumentAsString(Argument.First));
    }
}

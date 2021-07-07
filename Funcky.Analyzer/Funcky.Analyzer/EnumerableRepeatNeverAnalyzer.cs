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
    public class EnumerableRepeatNeverAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = nameof(EnumerableRepeatNeverAnalyzer);
        private const string Category = nameof(Funcky);

        private static readonly LocalizableString Title = LoadFromResource(nameof(EnumerableRepeatNeverAnalyzerTitle));
        private static readonly LocalizableString MessageFormat = LoadFromResource(nameof(EnumerableRepeatNeverAnalyzerMessageFormat));
        private static readonly LocalizableString Description = LoadFromResource(nameof(EnumerableRepeatNeverAnalyzerDescription));

        private static readonly DiagnosticDescriptor Rule = new(nameof(EnumerableRepeatNeverAnalyzer), Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(FindEnumerableRepeateNever, SyntaxKind.InvocationExpression);
        }

        private void FindEnumerableRepeateNever(SyntaxNodeAnalysisContext context)
        {
            if (IsRepeatNever(new SyntaxMatcher(context)))
            {
                context.ReportDiagnostic(CreateDiagnostic(context));
            }
        }

        private static bool IsRepeatNever(SyntaxMatcher syntax)
            => syntax.MatchStaticCall(nameof(Enumerable), nameof(Enumerable.Repeat))
                && syntax.MatchArgument(Argument.Second, 0);

        private static Diagnostic CreateDiagnostic(SyntaxNodeAnalysisContext context)
            => CreateDiagnostic(new SyntaxMatcher(context), (InvocationExpressionSyntax)context.Node);

        private static Diagnostic CreateDiagnostic(SyntaxMatcher syntax, InvocationExpressionSyntax invocationExpr)
            => Diagnostic.Create(Rule, invocationExpr.GetLocation(), syntax.GetArgumentAsString(Argument.First), syntax.GetArgumentType(Argument.First));
    }
}

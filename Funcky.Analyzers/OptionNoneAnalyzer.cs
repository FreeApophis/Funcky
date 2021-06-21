using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Funcky.Analyzers.Rules;

namespace Funcky.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class OptionNoneAnalyzer : DiagnosticAnalyzer
    {
        private const string NonePropertyName = "None";

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(ReplaceNoneMethodCallWithPropertyAccess);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var node = (InvocationExpressionSyntax)context.Node;

            if (node.Expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax
                && context.Compilation.GetGenericOptionType() is { } optionSymbol
                && context.SemanticModel.GetSymbolInfo(memberAccessExpressionSyntax.Expression) is { Symbol: INamedTypeSymbol { IsGenericType: true } symbol }
                && SymbolEqualityComparer.Default.Equals(optionSymbol, symbol.ConstructedFrom)
                && memberAccessExpressionSyntax.Name.Identifier.Text == NonePropertyName)
            {
                context.ReportDiagnostic(Diagnostic.Create(ReplaceNoneMethodCallWithPropertyAccess, node.GetLocation()));
            }
        }
    }
}

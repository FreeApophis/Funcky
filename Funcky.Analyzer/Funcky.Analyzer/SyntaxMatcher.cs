using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Funcky.Analyzer
{
    public class SyntaxMatcher
    {
        private const string DefaultValue = "NIL";
        private readonly SyntaxNodeAnalysisContext _analysisContext;

        public SyntaxMatcher(SyntaxNodeAnalysisContext analysisContext)
            => _analysisContext = analysisContext;

        private InvocationExpressionSyntax InvocationExpr => (InvocationExpressionSyntax)_analysisContext.Node;

        private SemanticModel SemanticModel => _analysisContext.SemanticModel;

        public bool MatchStaticCall(string className, string methodName)
            => InvocationExpr.Expression is MemberAccessExpressionSyntax memberAccessExpr
                && memberAccessExpr.Name.Identifier.ValueText == methodName
                && memberAccessExpr.Expression is IdentifierNameSyntax identifier
                && identifier.Identifier.ValueText == className;

        public bool MatchArgument<TArgument>(int argumentPosition, TArgument argumentValue)
            => GetArgument(argumentPosition) is { } argument
                && argument.Expression is LiteralExpressionSyntax literal
                && literal.Token.Value is TArgument value
                && value.Equals(argumentValue);

        public string GetArgumentAsString(int argumentPosition)
            => GetArgument(argumentPosition) is { } argument
                ? argument.ToString()
                : DefaultValue;

        internal string GetArgumentType(int argumentPosition)
            => GetArgument(argumentPosition) is { } argument
                ? SemanticModel.GetTypeInfo(argument.Expression).Type.ToDisplayString()
                : DefaultValue;

        private ArgumentSyntax? GetArgument(int argumentPosition)
            => InvocationExpr.ArgumentList is ArgumentListSyntax argumentList
                && argumentList.Arguments.Count > argumentPosition
                    ? argumentList.Arguments[argumentPosition]
                    : null;
    }
}

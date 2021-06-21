using Microsoft.CodeAnalysis;

namespace Funcky.Analyzers
{
    internal static class Rules
    {
        public static readonly DiagnosticDescriptor ReplaceNoneMethodCallWithPropertyAccess = new(
            id: "ƛ1001",
            title: "Replace method call to Option<TItem>.None() with property access",
            messageFormat: "Replace method call to Option<TItem>.None() with property access",
            category: "Funcky.Migration",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}

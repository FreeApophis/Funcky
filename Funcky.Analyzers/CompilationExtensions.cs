using Microsoft.CodeAnalysis;

namespace Funcky.Analyzers
{
    internal static class CompilationExtensions
    {
        public static INamedTypeSymbol? GetGenericOptionType(this Compilation compilation)
            => compilation.GetTypeByMetadataName("Funcky.Monads.Option`1");
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Funcky.SourceGenerator.VariadicFunctions
{
    internal abstract class NoOperationTemplateBase : IVariadicFunctionTemplate
    {
        public abstract string FormatMethodName();

        public abstract string FormatReturnType(IEnumerable<(string TypeParameterName, string ParameterName)> parameters);

        public abstract string FormatBody(IEnumerable<(string TypeParameterName, string ParameterName)> parameters);

        public string FormatTypeParameterName(int index, int arity) => $"T{index + 1}";

        public string FormatParameterName(int index, int arity) => $"p{index + 1}";

        public string FormatLeadingTrivia(IEnumerable<(string TypeParameterName, string ParameterName)> parameters)
            => "[System.Diagnostics.Contracts.Pure]\n";

        public string FormatModifiers() => "public static";

        public IEnumerable<string> FormatParameters(IEnumerable<(string TypeParameterName, string ParameterName)> parameters)
            => parameters.Select(p => $"{p.TypeParameterName} {p.ParameterName}");
    }
}

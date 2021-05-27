using System.Collections.Generic;
using System.Linq;

namespace Funcky.SourceGenerator.VariadicFunctions
{
    internal abstract class ConstantFunctionTemplate : IVariadicFunctionTemplate
    {
        public abstract string FormatMethodName();

        public abstract string FormatReturnValue();

        public string FormatTypeParameterName(int index, int arity) => arity == 1 ? "T" : $"T{index}";

        public string FormatParameterName(int index, int arity) => arity == 1 ? "ω" : $"ω{index}";

        public string FormatLeadingTrivia(IEnumerable<(string TypeParameterName, string ParameterName)> parameters)
            => $"/// <summary>A function that always returns <see langword=\"{FormatReturnValue()}\"/>.</summary>\n" +
               $"[System.Diagnostics.Contracts.Pure]\n";

        public string FormatModifiers() => "public static";

        public string FormatReturnType(IEnumerable<(string TypeParameterName, string ParameterName)> parameters) => "bool";

        public IEnumerable<string> FormatParameters(IEnumerable<(string TypeParameterName, string ParameterName)> parameters)
            => parameters.Select(p => $"{p.TypeParameterName} {p.ParameterName}");

        public string FormatBody(IEnumerable<(string TypeParameterName, string ParameterName)> parameters) => $"=> {FormatReturnValue()};";
    }
}

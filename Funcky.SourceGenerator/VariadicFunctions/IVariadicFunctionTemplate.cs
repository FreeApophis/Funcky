using System.Collections.Generic;

namespace Funcky.SourceGenerator.VariadicFunctions
{
    internal interface IVariadicFunctionTemplate
    {
        string FormatTypeParameterName(int index, int arity);

        string FormatParameterName(int index, int arity);

        string FormatLeadingTrivia(IEnumerable<(string TypeParameterName, string ParameterName)> parameters);

        string FormatModifiers();

        string FormatMethodName();

        string FormatReturnType(IEnumerable<(string TypeParameterName, string ParameterName)> parameters);

        IEnumerable<string> FormatParameters(IEnumerable<(string TypeParameterName, string ParameterName)> parameters);

        string FormatBody(IEnumerable<(string TypeParameterName, string ParameterName)> parameters);
    }
}

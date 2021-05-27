using System.Collections.Generic;

namespace Funcky.SourceGenerator.VariadicFunctions
{
    internal sealed class NoOperationTemplate : NoOperationTemplateBase
    {
        public override string FormatMethodName() => "NoOperation";

        public override string FormatReturnType(IEnumerable<(string TypeParameterName, string ParameterName)> parameters) => "void";

        public override string FormatBody(IEnumerable<(string TypeParameterName, string ParameterName)> parameters) => "{ }";
    }
}

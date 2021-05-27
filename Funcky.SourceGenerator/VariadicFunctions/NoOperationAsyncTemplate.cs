using System.Collections.Generic;

namespace Funcky.SourceGenerator.VariadicFunctions
{
    internal sealed class NoOperationAsyncTemplate : NoOperationTemplateBase
    {
        public override string FormatMethodName() => "NoOperationAsync";

        public override string FormatReturnType(IEnumerable<(string TypeParameterName, string ParameterName)> parameters) => "System.Threading.Tasks.Task";

        public override string FormatBody(IEnumerable<(string TypeParameterName, string ParameterName)> parameters) => "=> System.Threading.Tasks.Task.CompletedTask;";
    }
}

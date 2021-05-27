using System.Linq;
using Microsoft.CodeAnalysis;

namespace Funcky.SourceGenerator.VariadicFunctions
{
    [Generator]
    public sealed class VariadicFunctionsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            AddVariadicFunctionalFunction(context, instances: 8, new NoOperationTemplate());
            AddVariadicFunctionalFunction(context, instances: 8, new NoOperationAsyncTemplate());
            AddVariadicFunctionalFunction(context, instances: 4, new TrueTemplate());
            AddVariadicFunctionalFunction(context, instances: 4, new FalseTemplate());
        }

        private static void AddVariadicFunctionalFunction(GeneratorExecutionContext context, int instances, IVariadicFunctionTemplate template)
        {
            var source = $"namespace Funcky\n" +
                         $"#nullable enable\n" +
                         $"{{\n" +
                         $"    public static partial class Functional\n" +
                         $"    {{\n" +
                         RenderVariadicFunction(instances, template) +
                         $"    }}\n" +
                         $"}}\n";
            context.AddSource($"{template.FormatMethodName()}.Generated", source);
        }

        private static string RenderVariadicFunction(int instances, IVariadicFunctionTemplate template)
            => string.Join("\n", Enumerable.Range(0, instances + 1)
                .Select(arity => Enumerable.Range(0, arity)
                    .Select(n => (Type: template.FormatTypeParameterName(n, arity), Name: template.FormatParameterName(n, arity))))
                .Select(parameters => $"{template.FormatLeadingTrivia(parameters)}" +
                                      $"{template.FormatModifiers()}" +
                                      $" {template.FormatReturnType(parameters)}" +
                                      $" {template.FormatMethodName()}" +
                                      (parameters.Any()
                                          ? $"<{string.Join(", ", parameters.Select(p => p.Type))}>"
                                          : string.Empty) +
                                      $"({string.Join(", ", template.FormatParameters(parameters))})" +
                                      $" {template.FormatBody(parameters)}"));
    }
}

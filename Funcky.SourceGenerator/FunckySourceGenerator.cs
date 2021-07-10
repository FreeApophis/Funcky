using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Funcky.Money.SourceGenerator;
using Funcky.SourceGenerator.OrNoneGenerators;
using Microsoft.CodeAnalysis;

namespace Funcky.SourceGenerator
{
    [Generator]
    public sealed class FunckySourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            if (!Debugger.IsAttached)
            {
                ////Debugger.Launch();
            }
        }

        public void Execute(GeneratorExecutionContext context)
            => TryMethods()
                .Select(CreateOrNonePatternGenerator)
                .ToList()
                .ForEach(AddSource(context));

        private static Action<IOrNonePatternGenerator> AddSource(GeneratorExecutionContext context)
            => generator
                => context.AddSource(generator.HintName, generator.Source);

        private static IOrNonePatternGenerator CreateOrNonePatternGenerator(MethodInfo method)
            => method.Name switch
            {
                nameof(int.TryParse) => new TryParseGenerator(method),
                _ => throw new ArgumentException($"Generator does not handle '{method.Name}' methods.", nameof(method)),
            };

        private static IEnumerable<MethodInfo> TryMethods()
            => TryParseGenerator
                .GeneratableMethods();
    }
}

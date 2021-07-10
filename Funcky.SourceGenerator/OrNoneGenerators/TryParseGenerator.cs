using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Funcky.SourceGenerator.Extensions;
using Funcky.SourceGenerator.OrNoneGenerators;
using Funcky.SourceGenerator.Templates;
using Funcky.SourceGenerator.Templating;

namespace Funcky.Money.SourceGenerator
{
    internal class TryParseGenerator : IOrNonePatternGenerator
    {
        private readonly MethodInfo _method;

        public TryParseGenerator(MethodInfo method)
        {
            _method = method;
        }

        public string HintName
            => $"ParseExtensions.{FirstParametersTypeName().FirstToUpper()}.cs";

        public string Source
            => Template
                .Create(typeof(TryParseGenerator))
                .Get("ParseOrNone.template")
                .Apply(Substitutions(FirstParametersTypeName()));

        public static IEnumerable<MethodInfo> GeneratableMethods()
            => ParseTypes()
                .SelectMany(GetMethods)
                .Where(IsTryParseMethod)
                .Where(mi => mi.GetParameters().Length == 2);

        private static IEnumerable<TemplateSubstitution> Substitutions(string type)
            => new List<TemplateSubstitution>
            {
                new("type", type),
                new("typeLower", type.ToLower()),
                new("typeUpper", MethodNameType(type)),
            };

        private static bool IsTryParseMethod(MethodInfo mi)
            => mi.Name == nameof(int.TryParse);

        private static MethodInfo[] GetMethods(Type type)
            => type.GetMethods(BindingFlags.Public | BindingFlags.Static);

        private static IEnumerable<Type> ParseTypes()
            => new[]
            {
                typeof(bool),
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(DateTime),
                typeof(TimeSpan),
                ////typeof(Enum),
            };

        private string FirstParametersTypeName()
            => FirstParameterType()
                .ToBuiltInTypeName();

        private Type FirstParameterType()
            => _method
                .GetParameters()
                .Skip(1)
                .First()
                .ParameterType
                .GetElementType() ?? throw new NullReferenceException();

        private static string MethodNameType(string type)
            => type
                .FirstToUpper()
                .Replace("Bool", "Boolean");
    }
}

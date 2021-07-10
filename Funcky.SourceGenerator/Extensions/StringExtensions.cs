using System;

namespace Funcky.SourceGenerator.Extensions
{
    public static class StringExtensions
    {
        private static readonly Func<string, string> ToLower = s => s.ToLower();
        private static readonly Func<string, string> ToUpper = s => s.ToUpper();

        public static string FirstToUpper(this string type)
            => type.TransformFirstCharacter(ToUpper);

        public static string FirstToLower(this string type)
            => type.TransformFirstCharacter(ToLower);

        private static string TransformFirstCharacter(this string type, Func<string, string> transform)
            => transform(type.Substring(0, 1)) + type.Substring(1);
    }
}

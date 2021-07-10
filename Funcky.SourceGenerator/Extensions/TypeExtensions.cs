using System;

namespace Funcky.SourceGenerator.Extensions
{
    public static class TypeExtensions
    {
        public static string ToBuiltInTypeName(this Type type)
            => type switch
            {
                _ when type == typeof(bool) => "bool",
                _ when type == typeof(byte) => "byte",
                _ when type == typeof(sbyte) => "sbyte",
                _ when type == typeof(char) => "char",
                _ when type == typeof(decimal) => "decimal",
                _ when type == typeof(double) => "double",
                _ when type == typeof(float) => "float",
                _ when type == typeof(int) => "int",
                _ when type == typeof(uint) => "uint",
                _ when type == typeof(nint) => "nint",
                _ when type == typeof(nuint) => "nuint",
                _ when type == typeof(long) => "long",
                _ when type == typeof(ulong) => "ulong",
                _ when type == typeof(short) => "short",
                _ when type == typeof(ushort) => "ushort",
                _ when type == typeof(object) => "object",
                _ when type == typeof(string) => "string",
                _ when type == typeof(void) => "void",
                _ => type.Name,
            };
    }
}

using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using Funcky.Monads;

namespace Funcky.Extensions
{
    public static partial class ParseExtensions
    {
        [Pure]
        public static Option<byte> ParseByteOrNone(this string candidate, NumberStyles styles, IFormatProvider provider)
            => byte.TryParse(candidate, styles, provider, out var byteResult)
                ? byteResult
                : Option<byte>.None();

        [Pure]
        public static Option<short> ParseShortOrNone(this string candidate, NumberStyles styles, IFormatProvider provider)
            => short.TryParse(candidate, styles, provider, out var shortResult)
                ? shortResult
                : Option<short>.None();

        [Pure]
        public static Option<int> ParseIntOrNone(this string candidate, NumberStyles styles, IFormatProvider provider)
            => int.TryParse(candidate, styles, provider, out var integerResult)
                ? Option.Some(integerResult)
                : Option<int>.None();

        [Pure]
        public static Option<long> ParseLongOrNone(this string candidate, NumberStyles styles, IFormatProvider provider)
            => long.TryParse(candidate, styles, provider, out var longResult)
                ? longResult
                : Option<long>.None();

        [Pure]
        public static Option<float> ParseFloatOrNone(this string candidate, NumberStyles styles, IFormatProvider provider)
            => float.TryParse(candidate, styles, provider, out var doubleResult)
                ? Option.Some(doubleResult)
                : Option<float>.None();

        [Pure]
        public static Option<double> ParseDoubleOrNone(this string candidate, NumberStyles styles, IFormatProvider provider)
            => double.TryParse(candidate, styles, provider, out var doubleResult)
                ? doubleResult
                : Option<double>.None();

        [Pure]
        public static Option<decimal> ParseDecimalOrNone(this string candidate, NumberStyles styles, IFormatProvider provider)
            => decimal.TryParse(candidate, styles, provider, out var decimalResult)
                ? decimalResult
                : Option<decimal>.None();

        [Pure]
        public static Option<DateTime> ParseDateTimeOrNone(this string candidate, IFormatProvider provider, DateTimeStyles styles)
            => DateTime.TryParse(candidate, provider, styles, out var dateTime)
                ? dateTime
                : Option<DateTime>.None();

        [Pure]
        public static Option<TimeSpan> ParseTimeSpanOrNone(this string candidate, IFormatProvider provider)
            => TimeSpan.TryParse(candidate, provider, out var timeSpan)
                ? timeSpan
                : Option<TimeSpan>.None();

        [Pure]
        public static Option<TEnum> ParseEnumOrNone<TEnum>(this string candidate)
            where TEnum : struct
            => Enum.TryParse(candidate, out TEnum enumValue)
                ? enumValue
                : Option<TEnum>.None();

        [Pure]
        public static Option<TEnum> ParseEnumOrNone<TEnum>(this string candidate, bool ignoreCase)
            where TEnum : struct
            => Enum.TryParse(candidate, ignoreCase, out TEnum enumValue)
                ? enumValue
                : Option<TEnum>.None();
    }
}

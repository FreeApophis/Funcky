using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Funcky.Monads;

namespace Funcky.Extensions
{
    public static partial class StringExtensions
    {
        private delegate Option<SplitResult> ExtractElement(string text, int startIndex);

        private delegate Option<(int Index, int SeparatorLength)> FindNextIndex(string text, int startIndex);

        /// <summary>
        /// Splits a string into individual parts by a given separator.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="separator">A single character separating the parts.</param>
        /// <returns>A lazy IEnumerable containing the parts.</returns>
        [Pure]
        public static IEnumerable<string> SplitLazy(this string text, char separator)
            => text.SplitBy(ExtractByIndex(IndexOfCharSeparator(separator)));

        /// <summary>
        /// Splits a string into individual parts by several given separators.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="separators">Different characters separating the parts.</param>
        /// <returns>A lazy IEnumerable containing the parts.</returns>
        [Pure]
        public static IEnumerable<string> SplitLazy(this string text, params char[] separators)
            => text.SplitBy(ExtractByIndex(IndexOfCharSeparators(separators)));

        /// <summary>
        /// Splits a string into individual parts by a given separator.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="separator">A single string separating the parts.</param>
        /// <returns>A lazy IEnumerable containing the parts.</returns>
        [Pure]
        public static IEnumerable<string> SplitLazy(this string text, string separator)
            => text.SplitBy(ExtractByIndex(IndexOfStringSeparator(separator)));

        /// <summary>
        /// Splits a string into individual parts by several given separators.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="separators">Different strings separating the parts.</param>
        /// <returns>A lazy IEnumerable containing the parts.</returns>
        [Pure]
        public static IEnumerable<string> SplitLazy(this string text, params string[] separators)
            => text.SplitBy(ExtractByIndex(IndexOfStringSeparators(separators)));

        private static IEnumerable<string> SplitBy(this string text, ExtractElement extractNext)
            => Sequence
                    .Generate(new SplitResult(0), previous => extractNext(text, previous.NextStartIndex))
                    .Select(r => r.Result);

        private static FindNextIndex IndexOfCharSeparator(char separator)
            => (text, startIndex)
                => text.IndexOfOrNone(separator, startIndex).AndThen(index => (index, 1));

        private static FindNextIndex IndexOfCharSeparators(char[] separator)
            => (text, startIndex)
                => text.IndexOfAnyOrNone(separator, startIndex).AndThen(index => (index, 1));

        private static FindNextIndex IndexOfStringSeparator(string separator)
            => (text, startIndex)
                => separator.Length == 0
                    ? Option<(int Index, int SeparatorLength)>.None()
                    : text
                        .IndexOfOrNone(separator, startIndex, StringComparison.Ordinal)
                        .AndThen(index => (index, separator.Length));

        private static FindNextIndex IndexOfStringSeparators(string[] separators)
            => (text, startIndex)
                => separators
                    .AsEnumerable()
                    .Select(IndexOfAnyOrNone(text, startIndex))
                    .Aggregate(Option<(int Index, int SeparatorLength)>.None(), FindClosestSeparator);

        private static Option<(int Index, int SeparatorLength)> FindClosestSeparator(
            Option<(int Index, int SeparatorLength)> result,
            Option<(int Index, int SeparatorLength)> current)
            => result.Match(
                none: () => current,
                some: r => current.Match(
                    none: () => r,
                    some: Min(r)));

        private static Func<(int Index, int SeparatorLength), (int Index, int SeparatorLength)> Min((int Index, int SeparatorLength) result)
            => current
                => current.Index < result.Index
                    ? current
                    : result;

        private static Func<string, Option<(int Index, int SeparatorLength)>> IndexOfAnyOrNone(string text, int startIndex)
            => separator
                => separator.Length == 0
                    ? Option<(int Index, int SeparatorLength)>.None()
                    : text.IndexOfOrNone(separator, startIndex, StringComparison.Ordinal).AndThen(index => (index, separator.Length));

        private static ExtractElement ExtractByIndex(FindNextIndex findNextIndex)
            => ExtractBy(GetIndex(findNextIndex));

        // Why do we check here if there is a '<=' and not a '=='?
        // Simple example: ";".SplitLazy(';')?
        // * What is the length of this string? => 1
        // * How many values should we return? => 2
        // Since we want to return 2 values, we need to call ExtractElement with two distinct states: 0 and 1.
        // Therefore we are beyond the string when we are at the index of Length + 1.
        //
        // You can think of this as we use the index not for the character itself but for the position between the characters.
        //
        // "a;b" => [ ]a[ ];[ ]b[ ]
        //           ^   ^   ^   ^
        //           1   2   3   4
        private static ExtractElement ExtractBy(ExtractElement extractElement)
            => (text, startIndex)
                => startIndex <= text.Length
                    ? extractElement(text, startIndex)
                    : Option<SplitResult>.None();

        private static ExtractElement GetIndex(FindNextIndex getIndex)
            => (text, startIndex)
                => getIndex(text, startIndex)
                    .Match(
                        none: () => new SplitResult(text.Length + 1, text.Substring(startIndex)),
                        some: index => new SplitResult(index.Index + index.SeparatorLength, text.Substring(startIndex, index.Index - startIndex)));

        private readonly struct SplitResult
        {
            public readonly string Result;

            public readonly int NextStartIndex;

            public SplitResult(int nextStartIndex, Option<string> result = default)
                => (Result, NextStartIndex) = (result.GetOrElse(string.Empty), nextStartIndex);
        }
    }
}

using System;
using System.Diagnostics.Contracts;
#if SET_CURRENT_STACK_TRACE_SUPPORTED
using System.Runtime.ExceptionServices;
#if STACK_TRACE_HIDDEN_SUPPORTED
using System.Diagnostics;
#else
using System.Runtime.CompilerServices;
#endif
#else
using System.Diagnostics;
#endif

namespace Funcky.Monads
{
    public readonly partial struct Result<TValidResult> : IEquatable<Result<TValidResult>>
    {
        #if !SET_CURRENT_STACK_TRACE_SUPPORTED
        private const int SkipLowestStackFrame = 1;
        #endif

        private readonly TValidResult _result;
        private readonly Exception? _error;

        internal Result(TValidResult result)
            => (_result, _error) = (result, null);

        private Result(Exception error)
            => (_result, _error) = (default!, error);

        [Pure]
        public static bool operator ==(Result<TValidResult> lhs, Result<TValidResult> rhs)
            => lhs.Equals(rhs);

        [Pure]
        public static bool operator !=(Result<TValidResult> lhs, Result<TValidResult> rhs)
            => !lhs.Equals(rhs);

        #if SET_CURRENT_STACK_TRACE_SUPPORTED
        // Methods with AggressiveInlining are always excluded from the stack trace.
        // This is required for <c>SetCurrentStackTrace</c> to work properly.
        // See: https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/Diagnostics/StackTrace.cs#L347
        #if STACK_TRACE_HIDDEN_SUPPORTED
        [StackTraceHidden]
        #else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #endif
        #endif
        public static Result<TValidResult> Error(Exception item)
        {
            #if SET_CURRENT_STACK_TRACE_SUPPORTED
            ExceptionDispatchInfo.SetCurrentStackTrace(item);
            #else
            item.SetStackTrace(new StackTrace(SkipLowestStackFrame, true));
            #endif

            return new Result<TValidResult>(item);
        }

        [Pure]
        public TMatchResult Match<TMatchResult>(Func<TValidResult, TMatchResult> ok, Func<Exception, TMatchResult> error)
            => _error is null
                ? ok(_result)
                : error(_error);

        public void Match(Action<TValidResult> ok, Action<Exception> error)
        {
            if (_error is null)
            {
                ok(_result);
            }
            else
            {
                error(_error);
            }
        }

        [Pure]
        public override bool Equals(object? obj)
            => obj is Result<TValidResult> other && Equals(other);

        [Pure]
        public bool Equals(Result<TValidResult> other)
            => Equals(_result, other._result)
               && Equals(_error, other._error);

        [Pure]
        public override int GetHashCode()
            => Match(
                ok: result => result?.GetHashCode(),
                error: error => error.GetHashCode()) ?? 0;
    }

    public static class Result
    {
        [Pure]
        public static Result<TValidResult> Ok<TValidResult>(TValidResult item)
            => new(item);

        [Pure]
        public static Result<TValidResult> Return<TValidResult>(TValidResult item)
            => new(item);
    }
}

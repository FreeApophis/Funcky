using System;
using static Funcky.Functional;

namespace Funcky.Monads
{
    public delegate T Io<out T>();

    public static class Io
    {
        public static Io<TResult> Return<TResult>(Func<TResult> function)
            => ()
                => function();

        public static Io<Unit> Return(Action action)
            => new(ActionToUnit(action));

        public static Io<TSource> Return<TSource>(TSource value)
            => ()
                => value;
    }
}

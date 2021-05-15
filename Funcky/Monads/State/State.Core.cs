namespace Funcky.Monads
{
    public delegate (T Value, TState State) State<TState, T>(TState state);

    public static class State
    {
        public static State<TState, TState> GetState<TState>()
            => oldState
                => (oldState, oldState);

        public static State<TState, Unit> SetState<TState>(TState newState)
            => _
                => (default, newState);
    }
}

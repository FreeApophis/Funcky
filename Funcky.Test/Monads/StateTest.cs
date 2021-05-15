using Funcky.Monads;
using Xunit;
using static Funcky.Monads.State;

namespace Funcky.Test.Monads
{
    public sealed class StateTest
    {
        [Fact]
        public void DefineWorkFlowWithStatesProducesDesiredResult()
        {
            const string initialState = nameof(initialState);
            const string newState = nameof(newState);
            const string resetState = nameof(resetState);

            State<string, int> source1 = oldState => (1, oldState);
            State<string, bool> source2 = _ => (true, newState);
            State<string, char> source3 = '@'.State<string, char>();

            State<string, string[]> query =
                from value1 in source1
                from state1 in GetState<string>()
                from value2 in source2
                from state2 in GetState<string>()
                from unit in SetState(resetState)
                from state3 in GetState<string>()
                from value3 in source3
                select new[] { state1, state2, state3 };

            var (states, state) = query(initialState);

            Assert.Equal(new[] { initialState, newState, resetState }, states);
            Assert.Equal(resetState, state);
        }
    }
}

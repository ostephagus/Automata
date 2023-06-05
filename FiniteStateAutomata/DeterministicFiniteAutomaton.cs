namespace FiniteStateAutomata
{
    public class DeterministicFiniteAutomaton<TState, TSymbol>
    {
        private TState[] states;
        private TState startState;
        private TSymbol[] alphabet;
        private TState[] endStates;
        private Dictionary<(TState, TSymbol), TState> transitionFunction;

        public TState[] States { get => states; set => states = value; }
        public TState StartState { get => startState; set => startState = value; }
        public TSymbol[] Alphabet { get => alphabet; set => alphabet = value; }
        public TState[] EndStates { get => endStates; set => endStates = value; }
        public Dictionary<(TState, TSymbol), TState> TransitionFunction { get => transitionFunction; set => transitionFunction = value; }

        public DeterministicFiniteAutomaton(TState[] states, TState startState, TSymbol[] alphabet, TState[] endStates, Dictionary<(TState, TSymbol), TState> transitionFunction)
        {
            States = states;
            StartState = startState;
            Alphabet = alphabet;
            EndStates = endStates;
            TransitionFunction = transitionFunction;
        }

        public bool Run(TSymbol[]? word)
        {
            if (word == null)
            {
                return endStates.Contains(startState); //If the empty set is given, return whether the start state is an end state
            }

            TState state = StartState;
            foreach (TSymbol symbol in word)
            {
                if (!alphabet.Contains(symbol))
                {
                    return false;
                }
                state = transitionFunction[(state, symbol)];
            }
            return endStates.Contains(state);
        }
    }
}

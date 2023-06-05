namespace FiniteStateAutomata
{
    public class NonDeterministicFiniteAutomaton<TState, TSymbol> where TState : IEquatable<TState>
    {
        private TState[] states;
        private TState[] startStates;
        private TSymbol[] alphabet;
        private TState[] endStates;
        private Dictionary<(TState, TSymbol), TState[]?> transitionRelations;

        private DeterministicFiniteAutomaton<TState[], TSymbol>? translatedDFA;
        private bool needsTranslating;

        public TState[] States { get => states; set { 
                states = value;
                needsTranslating = true; 
            } }
        public TState[] StartStates { get => startStates; set {
                startStates = value;
                needsTranslating = true;
            } }
        public TSymbol[] Alphabet { get => alphabet; set {
                alphabet = value;
                needsTranslating = true;
            } }
        public TState[] EndStates { get => endStates; set {
                endStates = value;
                needsTranslating = true;
            } }
        public Dictionary<(TState, TSymbol), TState[]?> TransitionRelations { get => transitionRelations; set {
                transitionRelations = value;
                needsTranslating = true;
            } }

        public NonDeterministicFiniteAutomaton(TState[] states, TState[] startStates, TSymbol[] alphabet, TState[] endStates, Dictionary<(TState, TSymbol), TState[]?> transitionRelations)
        {
            States = states;
            StartStates = startStates;
            Alphabet = alphabet;
            EndStates = endStates;
            TransitionRelations = transitionRelations;
            needsTranslating = true;
        }

        private void Translate()
        {
            Dictionary<(TState[], TSymbol), TState[]> dfaTransitionFunction = new(); //Create a new dictionary to hold the state transitions for the DFA. The difference is that the state type is a set of TState from the NFA
            List<TState[]> dfaStates = new(); //List of states for the DFA, to be used for PopulateTransitions
            List<TState[]> dfaEndStates = new();

            dfaStates.Add(startStates); //Create a state to represent all possible start states
            PopulateTransitions(dfaTransitionFunction, dfaStates, startStates, dfaEndStates); //Recursively populate the states for the DFA

            translatedDFA = new DeterministicFiniteAutomaton<TState[], TSymbol>(dfaStates.ToArray(), StartStates, alphabet, dfaEndStates.ToArray(), dfaTransitionFunction);
        }

        private void PopulateTransitions(Dictionary<(TState[], TSymbol), TState[]> transitionFunction, List<TState[]> states, TState[] originalState, List<TState[]> dfaEndStates)
        {
            for (int i = 0; i < alphabet.Length; i++) //Loop through each symbol in the alphabet
            {
                List<TState> newState = new();
                foreach (TState substate in originalState) //Loop through each position that the NFA has pointers on
                {
                    TState[]? nextSubstate = transitionRelations[(substate, alphabet[i])]; //Perform the "moving" of a pointer according to the NFA's transition relation, and figure out how this affects the DFA
                    if (nextSubstate != null)
                    {
                        newState.AddRange(nextSubstate); //Build the new state by adding all the possible states for this pointer movement
                    }
                }
                TState[] newStateArray = newState.ToArray();

                foreach (TState endState in endStates) //If the new state for the DFA contains one of the NFA's end states, make this an end state for the DFA.
                {
                    if (newStateArray.Contains(endState))
                    {
                        if (!JaggedContains(dfaEndStates, newStateArray)) //Add the state if it doesn't already exist
                        {
                            dfaEndStates.Add(newStateArray);
                        }
                        break;
                    }
                }

                transitionFunction.Add((originalState, alphabet[i]), newStateArray); //Add the link from the old state to the new state

                if (!JaggedContains(states, newStateArray)) //If the new state doesn't yet exist, create it and populate its transitions.
                {
                    states.Add(newStateArray);
                    PopulateTransitions(transitionFunction, states, newStateArray, dfaEndStates); //Recursive call to populate the new state's transitions
                }
            }
        }

        public static bool JaggedContains(List<TState[]> JaggedArray, TState[] ComparisonArray)
        {
            foreach (TState[] subArray in JaggedArray)
            {
                if (subArray.Length == ComparisonArray.Length)
                {
                    int i = 0;
                    int arrayLength = subArray.Length;
                    while (i < arrayLength && subArray[i].Equals(ComparisonArray[i])) //Loop through all the elements in each array
                    {
                        i++;
                    }
                    if (i == arrayLength) //If the loop gets to the end, all the elements are equal.
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Run(TSymbol[]? word)
        {
            if (needsTranslating)
            {
                Translate(); //Translate if needed
            }
            needsTranslating = false;

            return translatedDFA.Run(word); //Use the translated DFA to run and use its return value
        }
    }
}

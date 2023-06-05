namespace FiniteStateAutomata
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestNonDeterministicFiniteAutomaton();
        }

        static void TestNonDeterministicFiniteAutomaton()
        {
            NonDeterministicFiniteAutomaton<int, char> nfa = new(new int[] { 0, 1, 2 }, new int[] { 0 }, new char[] { '0', '1' }, new int[] { 2 }, new Dictionary<(int, char), int[]?>
            {
                { (0, '0'), new int[] { 0 } },
                { (0, '1'), new int[] { 0, 1 } },
                { (1, '0'), new int[] { 2 } },
                { (1, '1'), new int[] { 2 } },
                { (2, '0'), null },
                { (2, '1'), null }
            });

            Console.WriteLine($"010: {nfa.Run("010".ToCharArray())}");
            Console.WriteLine($"111: {nfa.Run("111".ToCharArray())}");
            Console.WriteLine($"empty: {nfa.Run(null)}");
            Console.WriteLine($"000: {nfa.Run("000".ToCharArray())}");
            Console.WriteLine($"111100: {nfa.Run("111100".ToCharArray())}.");
        }

        static void TestDeterministicFiniteAutomaton()
        {
            DeterministicFiniteAutomaton<int, char> dfa = new(new int[] { 0, 1, 2 }, 0, new char[] { 'a', 'b' }, new int[] { 0, 1 }, new Dictionary<(int, char), int>
            {
                { (0, 'a'), 0 },
                { (0, 'b'), 1 },
                { (1, 'a'), 2 },
                { (1, 'b'), 1 },
                { (2, 'a'), 2 },
                { (2, 'b'), 2 }
            });
            Console.WriteLine($"aaa: {dfa.Run("aaa".ToCharArray())}");
            Console.WriteLine($"bbb: {dfa.Run("bbb".ToCharArray())}");
            Console.WriteLine($"empty: {dfa.Run(null)}");
            Console.WriteLine($"aaabb: {dfa.Run("aaabb".ToCharArray())}");
            Console.WriteLine($"aaabba: {dfa.Run("aaabba".ToCharArray())}.");
        }
        static void TestJaggedArrayContains()
        {
            int[][] jaggedArray = { new int[] { 0, 1 }, new int[] { 0, 2 }, new int[] { 1, 2 } };
            Console.WriteLine(jaggedArray.Contains(new int[] { 0, 1 }));
            Console.WriteLine(jaggedArray.Contains(new int[] { 0 }));
            Console.WriteLine(jaggedArray.Contains(new int[] { 0, 2 }));
        }
    }
}
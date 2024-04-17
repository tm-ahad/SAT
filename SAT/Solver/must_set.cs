namespace SAT.Solver
{
    public class MustSet
    {
        public HashSet<string> set = new HashSet<string>();
        private bool containsNot;
        private bool containsNor;

        public bool Add(string s)
        {
            bool startWith = s.StartsWith("!");
            string notS = startWith ? s : $"!{s}";

            if (startWith ? containsNor : containsNot)
            {
                if (set.Contains(notS))
                {
                    return true;
                }

                set.Add(s);
            }

            containsNot |= startWith;
            containsNor |= !startWith;

            return false;
        }

        public bool Merge(MustSet other)
        {
            foreach (string el in other.set)
            {
                if (Add(el)) return true;
            }

            return false;
        }
    }
}

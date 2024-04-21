using System.Timers;

namespace SAT.Solver
{
    public class StrictRequirement
    {
        public HashSet<string> set = new HashSet<string>();
        private bool containsNot;
        private bool containsNor;

        public bool Add(string s)
        {
            bool startWith = s.StartsWith("!");
            string notS = startWith ? s[1..] : $"!{s}";

            if ((startWith ? containsNor : containsNot) && set.Contains(notS)) return true;
            set.Add(s);

            containsNot |= startWith;
            containsNor |= !startWith;

            return false;
        }
        public void Not()
        {
            foreach (string el in set)
            {
                string inv = el.StartsWith("!") ? el[1..] : $"!{el}";
                set.Remove(el);
                set.Add(inv);
            }
        }

        public bool Merge(StrictRequirement other)
        {
            foreach (string el in other.set)
            {
                if (Add(el)) return true;
            }
            return false;
        }
    }

    public class OptionalRequirement
    {
        public HashSet<string> set = new HashSet<string>();
        public int n;

        private bool containsNot;
        private bool containsNor;

        public void Add(string s)
        {
            bool startWith = s.StartsWith("!");
            string notS = startWith ? s[1..] : $"!{s}";

            if (set.Contains(notS)) set = [];
            set.Add(s);

            containsNot |= startWith;
            containsNor |= !startWith;
        }

        public void Merge(OptionalRequirement other)
        {
            HashSet<string> s = other.set.Count() > set.Count() ? other.set : set;
            HashSet<string> b = other.set.Count() <= set.Count() ? other.set : set;

            foreach (string el in s)
            {
                if (el.StartsWith("!") && containsNor && s.Contains(el[1..]))
                {
                    set = new HashSet<string>();
                    break;
                }
                else if (!el.StartsWith("!") && containsNot && other.set.Contains($"!{s}"))
                {
                    set = new HashSet<string>();
                    break;
                }
                Add(el);
            }
        }

        public StrictRequirement Not()
        {
            StrictRequirement res = new StrictRequirement();

            foreach (string el in set)
            {
                string inv = el.StartsWith("!") ? el[1..] : $"!{el}";
                res.Add(inv);
            }

            return res;
        }
    }
}

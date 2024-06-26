namespace SAT.Solver
{
    public class GroupSet(string inc)
    {
        private List<HashSet<string>> xclusive = [[]];
        private List<HashSet<string>> inclusive = [[inc]];

        private static HashSet<string> AddToGroup(HashSet<string> group, string a)
        {
            if (group.Contains(CSolver.InvertS(a)))
            {
                group.Clear();
            }
            else
            {
                group.Add(a);
            }

            return group;
        }

        private static HashSet<string> UnionGroupSet(HashSet<string> a, HashSet<string> b)
        {
            HashSet<string> small = a.Count < b.Count ? a : b;
            HashSet<string> large = small == a ? b : a;

            foreach (string e in small)
            {
                large = AddToGroup(large, e);
                if (large.Count == 0)
                {
                    return [];
                }
            }

            return large;
        }

        private static List<HashSet<string>> MultiplyGroupSet(List<HashSet<string>> gs1, List<HashSet<string>> gs2)
        {
            List<HashSet<string>> mgs = [];

            foreach (HashSet<string> g1 in gs1)
            {
                foreach (HashSet<string> g2 in gs2)
                {
                    HashSet<string> union = UnionGroupSet(g1, g2);
                    if (union.Count != 0) mgs.Add(union);
                }
            }

            return mgs;
        }

        private static List<HashSet<string>> AddGroupSet(List<HashSet<string>> gs1, List<HashSet<string>> gs2)
        {
            if (gs1[0].Count == 0 || gs2[0].Count == 0)
            {
                return gs1[0].Count == 0 ? gs2 : gs1;
            }
            else
            {
                return [.. gs1, .. gs2];
            }
        }

        public void Or(GroupSet b) 
        {
            xclusive = MultiplyGroupSet(xclusive, b.xclusive);
            inclusive = AddGroupSet(inclusive, b.inclusive);
        }

        public void And(GroupSet b) 
        {
            inclusive = MultiplyGroupSet(inclusive, b.inclusive);
            xclusive = AddGroupSet(xclusive, b.xclusive);
        }

        public void Not() => (xclusive, inclusive) = (inclusive, xclusive);

        public bool IsSatisfiable(int vars)
        {
            List<HashSet<string>> small = inclusive.Count < xclusive.Count ? inclusive : xclusive;
            List<HashSet<string>> other = small == inclusive ? xclusive : inclusive;

            foreach (HashSet<string> e in small)
            {
                if (other.Contains(e)) 
                {
                    Console.WriteLine("BRUH NIGGLAR");
                    return false;
                };
            }

            if (inclusive.Count > 0) return true;

            var xclusiveCount = 0;
            foreach (HashSet<string> g in xclusive)
            {
                xclusiveCount += 1 >> (vars - g.Count);
            }

            return xclusiveCount < (1 << vars);
        }
    }
}
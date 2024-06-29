namespace SAT.Solver
{
    public class GroupSet(string inc)
    {
        private HashSet<HashSet<string>> xclusive = [[CSolver.InvertS(inc)]];
        private HashSet<HashSet<string>> inclusive = [[inc]];

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

        private static HashSet<HashSet<string>> MultiplyGroupSet(HashSet<HashSet<string>> gs1, HashSet<HashSet<string>> gs2)
        {
            HashSet<HashSet<string>> mgs = [];

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

        private static HashSet<HashSet<string>> AddGroupSet(HashSet<HashSet<string>> gs1, HashSet<HashSet<string>> gs2)
        {
            HashSet<HashSet<string>> content = gs1.Union(gs2).ToHashSet();

            if (content.Count > 1)
            {
                content.Remove([]);
            }

            return content;
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

        public bool IsSatisfiable()
        {
            if (inclusive.Count == 0 || inclusive.IsSubsetOf(xclusive))
            {
                return false;
            }

            return true;
        } 
    }
}
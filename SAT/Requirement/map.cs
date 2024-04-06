namespace SAT.Req
{
    using SAT.Consts;
    using SAT.Gate;

    using Solution = Gate.CGate<Tuple<HashSet<char>, int, int>>;

    public class RequirementMap
    {
        public GateType type;
        public List<RequirementMap> list = [];
        public Dictionary<HashSet<char>, HashSet<uint>> dict = [];


        public static Solution CombineOR(Solution a, Solution b)
        {
            if (isNone) 
            {
                if (a.First().dict[[]].Contains(Const.AlwaysPossible)) 
                {
                    return Tuple.Create(true, b);
                } 
                else if (b.First().dict[[]].Contains(Const.AlwaysPossible))
                {
                    return Tuple.Create(true, a);
                }

                return Tuple.Create(false, b);
            }

            if (a.First().dict[[]].Contains(Const.NotPossible))
            {
                return Tuple.Create(true, b);
            }
            else if (b.First().dict[[]].Contains(Const.NotPossible))
            {
                return Tuple.Create(true, a);
            }

            return Tuple.Create(false, b);
        }

        public void MergeAND(RequirementMap other) 
        {
            Tuple<bool, List<RequirementMap>> sol = CheckEitherNone(other.list, list, true);

            if (sol.Item1)
            {
                list = sol.Item2;
            }
            else 
            {
                Tuple<bool, List<RequirementMap>> sol2 = CheckEitherNone(other.list, list, false);

                if (sol.Item1)
                {
                    list = [];
                    dict = new Dictionary<HashSet<char>, HashSet<uint>>()
                    {
                        { [], [Const.NotPossible] }
                    };
                }
                else
                {
                    if (other.)

                    list = [this, other];
                    type = GateType.AND;
                }
            }
        }
    }
}

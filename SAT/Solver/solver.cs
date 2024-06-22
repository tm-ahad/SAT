using SAT.Gate;

namespace SAT.Solver
{
    public class CSolver(int _vars)
    {
        private const uint MinimumExpressionLengthRequiredForCaching = 5;
        private readonly Dictionary<string, SolutionData> cache = [];
        private readonly int vars = _vars;

        public static string InvertS(string s) 
        {
            if (s.StartsWith('!'))
            {
                return s[1..];
            }
            else
            {
                return $"!{s}";
            }
        }

        public SolutionData Solve(CGate tree)
        {
            if (tree.Variable != null)
            {
                GroupSet gr = new();
                gr.AddInclsive(tree.Variable);

                return new SolutionData()
                {
                    Satisfiction = true,
                    GroupSet = gr
                };
            }

            if (cache.TryGetValue(tree.Literal, out SolutionData sol))
            {
                return sol;
            }

            switch (tree.Type)
            {
                case GateType.AND:

                    SolutionData leftSol = Solve(tree.Left);
                    SolutionData rightSol = Solve(tree.Right);

                    leftSol.GroupSet.And(rightSol.GroupSet);

                    sol = new SolutionData()
                    {
                        Satisfiction = tree.Root ? leftSol.GroupSet.IsSatisfiable(vars) : null,
                        GroupSet = leftSol.GroupSet,
                    };
                    break;

                case GateType.OR:

                    SolutionData leftSol_ = Solve(tree.Left);
                    SolutionData rightSol_ = Solve(tree.Right);

                    leftSol_.GroupSet.Or(rightSol_.GroupSet);

                    sol = new SolutionData()
                    {
                        Satisfiction = tree.Root ? leftSol_.GroupSet.IsSatisfiable(vars) : null,
                        GroupSet = leftSol_.GroupSet,
                    };
                    break;

                case GateType.NOT:

                    SolutionData SolveLeft = Solve(tree.Left);
                    SolveLeft.GroupSet.Not();

                    sol = new SolutionData()
                    {
                        Satisfiction = tree.Root ? SolveLeft.GroupSet.IsSatisfiable(vars) : null,
                        GroupSet = SolveLeft.GroupSet,
                    };

                    break;

                default:
                    throw new Exception("Invalid gate type");
            }

            if (tree.Root)
            {
                sol.Satisfiction = sol.IsSatisfiable();
            }

            if (tree.Literal.Length >= MinimumExpressionLengthRequiredForCaching)
            {
                cache.Add(tree.Literal, sol);
            }
            return sol;

            throw new Exception("Impossible possibily!");
        }
    }
}

using SAT.Gate;

namespace SAT.Solver
{
    public class CSolver
    {
        public static SolutionData Solve(CGate tree)
        {
            if (tree.Variable != null)
            {
                HashSet<string> vars = [tree.Variable];
                MustSet mustSet = new MustSet();
                mustSet.Add(tree.Variable);

                return new SolutionData()
                {
                    satisfiction = ESaticfiction.Both,
                    mustSet = mustSet,
                    Variables = vars
                };
            }

            switch (tree.Type)
            {
                case GateType.AND:

                    SolutionData leftSol = Solve(tree.Left);
                    SolutionData rightSol = Solve(tree.Right);

                    leftSol.Variables.UnionWith(rightSol.Variables);

                    if (!leftSol.mustSet.Merge(rightSol.mustSet)) 
                    {
                        return SolutionData.NotSatisfiable();
                    };
                    leftSol.mustSet.Add(tree.Literal);

                    return new SolutionData()
                    {
                        satisfiction = CSaticfiction.AND(leftSol.satisfiction, rightSol.satisfiction),
                        Variables = leftSol.Variables,
                        mustSet = leftSol.mustSet
                    };

                case GateType.OR:

                    SolutionData solveLeft = Solve(tree.Left);
                    SolutionData solveRight = Solve(tree.Right);

                    solveLeft.Variables.UnionWith(solveRight.Variables);
                    if (solveLeft.satisfiction == ESaticfiction.None || solveRight.satisfiction == ESaticfiction.None)
                    {
                        return solveLeft.satisfiction == ESaticfiction.None ? solveRight : solveLeft;
                    }

                    MustSet mustSet = new MustSet();
                    mustSet.Add(tree.Literal);

                    return new SolutionData() 
                    {
                        Variables = solveLeft.Variables,
                        satisfiction = CSaticfiction.OR(solveLeft.satisfiction, solveRight.satisfiction),
                        mustSet = mustSet
                    };

                case GateType.NOT:

                    SolutionData SolveLeft = Solve(tree.Left);
                    MustSet mustSet0 = new MustSet();
                    _ = mustSet0.Add(tree.Literal);

                    SolveLeft.satisfiction = CSaticfiction.NOT(SolveLeft.satisfiction);
                    SolveLeft.mustSet = mustSet0;
                    return SolveLeft;
            }

            throw new Exception("Impossible possibily!");
        }
    }
}

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
                OptionalRequirement orSet = new OptionalRequirement();
                StrictRequirement mustSet = new StrictRequirement();


                mustSet.Add(tree.Variable);
                orSet.Add(tree.Variable);

                return new SolutionData()
                {
                    satisfiction = ESaticfiction.Both,
                    mustSet = mustSet,
                    Variables = vars,
                    orSet = orSet
                };
            }

            switch (tree.Type)
            {
                case GateType.AND:

                    SolutionData leftSol = Solve(tree.Left);
                    SolutionData rightSol = Solve(tree.Right);

                    leftSol.Variables.UnionWith(rightSol.Variables);

                    if (leftSol.mustSet.Merge(rightSol.mustSet)) 
                    {
                        return SolutionData.NotSatisfiable();
                    };

                    leftSol.mustSet.Add(tree.Right.Literal);
                    leftSol.mustSet.Add(tree.Left.Literal);

                    return new SolutionData()
                    {
                        satisfiction = CSaticfiction.AND(leftSol.satisfiction, rightSol.satisfiction),
                        Variables = leftSol.Variables,
                        mustSet = leftSol.mustSet,
                        orSet = leftSol.orSet
                    };

                case GateType.OR:

                    SolutionData solveLeft = Solve(tree.Left);
                    SolutionData solveRight = Solve(tree.Right);

                    solveLeft.Variables.UnionWith(solveRight.Variables);
                    solveLeft.orSet.Merge(solveRight.orSet);
                    if (solveLeft.satisfiction == ESaticfiction.None || solveRight.satisfiction == ESaticfiction.None)
                    {
                        return solveLeft.satisfiction == ESaticfiction.None ? solveRight : solveLeft;
                    }

                    StrictRequirement mustSet = new StrictRequirement();

                    mustSet.Add(tree.Right.Literal);
                    mustSet.Add(tree.Left.Literal);

                    return new SolutionData() 
                    {
                        Variables = solveLeft.Variables,
                        satisfiction = CSaticfiction.OR(solveLeft.satisfiction, solveRight.satisfiction),
                        orSet = solveLeft.orSet,
                        mustSet = mustSet
                    };

                case GateType.NOT:

                    SolutionData SolveLeft = Solve(tree.Left);
                    
                    if (SolveLeft.mustSet.Merge(SolveLeft.orSet.Not()))
                    {
                        SolveLeft = SolutionData.NotSatisfiable();
                    }
                    
                    SolveLeft.mustSet.Not();
                    SolveLeft.satisfiction = CSaticfiction.NOT(SolveLeft.satisfiction);

                    return SolveLeft;
            }

            throw new Exception("Impossible possibily!");
        }
    }
}

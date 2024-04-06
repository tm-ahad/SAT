using SAT.Gate;
using SAT.OM;

namespace SAT.Solver
{
    public class CSolver
    {
        public string? varName;

        public SolutionData Solve(CGate tree, bool isNOT)
        {
            if (tree.VariableName != null)
            {
                HashSet<string> vars = [tree.VariableName];
                vars.

                if (varName == tree.VariableName)
                {
                    return new SolutionData 
                    {
                        isSatisfiable = true,
                        isNot = isNOT,
                        req = new
                        Variables = vars
                    };
                }
                else 
                {
                    varName = tree.VariableName;
                    return new SolutionData
                    {
                        OM = new IOperandMatch.Least(1, true),
                        isSatisfiable = true,
                        Variables = vars
                    };
                }
            }

            GateType gateType = tree.Type ?? GateType.NONE;

            switch (gateType)
            {
                case GateType.AND:

                    SolutionData leftSol = Solve(tree.Left);
                    SolutionData rightSol = Solve(tree.Right);

                    if ()

                    HashSet<string> leftVars = leftSol.Variables;
                    HashSet<string> rightVars = rightSol.Variables;
                    HashSet<string> intersect = rightSol.Variables.Intersect(leftSol.Variables);
                    HashSet<string> combinedVars = leftVars.Union(rightVars).ToHashSet();

                    if (intersect.Count == 0)
                    {
                        return new SolutionData()
                        {
                            isSatisfiable = leftSol.isSatisfiable & rightSol.isSatisfiable,
                            Variables = combinedVars,
                            req: lef
                        }
                    }

                    IOperandMatch OM;
                    int leftSolLr = leftSol.OM.LeastVar(leftSol.Variables.Count);
                    int rightSolLr = leftSol.OM.LeastVar(leftSol.Variables.Count);

                    int intersectionLength = leftVars.Intersect(rightVars).Count();

                    int maxLr = Math.Max(leftSolLr, rightSolLr);
                    IOperandMatch maxOM = maxLr == leftSolLr ? leftSol.OM : rightSol.OM;

                    if (intersectionLength >= maxLr) 
                    {
                        OM = maxOM;
                    }
                    else 
                    {
                        bool a
                        IOperandMatch lst = new Least((leftSolLr + rightSolLr) - intersectionLengthm, )
                    }


                    List<IOperandMatch> OMS = SetsAreEqual(leftSol.Variables, rightSol.Variables) ? leftSol.OM.High(rightSol.OM) :  ;
                    bool isSAT = !SetsAreEqual(leftSol.Variables, rightSol.Variables) || SAT.Any();
               
                    leftSol.Variables.UnionWith(other: rightSol.Variables);
                    var mergedVars = leftSol.Variables;

                    return new SolutionData
                    {
                        OMS = SAT,
                        isSatisfiable = isSAT,
                        Variables = mergedVars
                    };

                case GateType.OR:

                    SolutionData solveLeft = Solve(tree.Left);
                    SolutionData solveRight = Solve(tree.Right);

                    Requirement r = solveLeft.req.Low(solveRight.req);
                    SolutionData ret = r == solveLeft.req ? solveLeft : solveRight;

                    return ret;

                case GateType.NOT:
                    SolutionData SolveLeft = Solve(tree.Left);
                    SolveLeft.req = SolveLeft.req.Invert();
                    return SolveLeft;
            }

            throw new Exception("Impossible possibily!");
        }
    }
}

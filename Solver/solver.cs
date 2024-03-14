using SAT.Gate;
using SAT.OM;

namespace SAT.Solver
{
    public class CSolver
    {
        public string? varName;

        static List<EOperandMatch> CommonOM(List<EOperandMatch> a, List<EOperandMatch> b)
        {
            return a.Intersect(b).ToList();
        }

        static bool SetsAreEqual(HashSet<string> a, HashSet<string> b)
        {
            if (a.Count != b.Count) return false;
            return a.SetEquals(b);
        }

        public SolutionData Solve(CGate tree)
        {
            if (tree.VariableName != null)
            {
                HashSet<string> vars = [tree.VariableName];
                List<EOperandMatch> oms = [EOperandMatch.Diff, EOperandMatch.Same1];

                if (varName == tree.VariableName)
                {
                    return new SolutionData 
                    {
                        OMS = oms,
                        isSatisfiable = true,
                        Variables = vars
                    };
                }
                else 
                {
                    varName = tree.VariableName;
                    return new SolutionData
                    {
                        OMS = oms,
                        isSatisfiable = true,
                        Variables = vars
                    };
                }
            }

            GateType gateType = tree.Type ?? GateType.NONE;

            switch (gateType)
            {
                case GateType.AND:

                    var leftSol = Solve(tree.Left);
                    var rightSol = Solve(tree.Right);

                    List<EOperandMatch> SAT = CommonOM(leftSol.OMS, rightSol.OMS);
                    bool isSAT = SetsAreEqual(leftSol.Variables, rightSol.Variables) ? SAT.Any() : true;
               
                    leftSol.Variables.UnionWith(rightSol.Variables);
                    var mergedVars = leftSol.Variables;

                    return new SolutionData
                    {
                        OMS = SAT,
                        isSatisfiable = isSAT,
                        Variables = mergedVars
                    };

                case GateType.OR:

                    var solveLeft = Solve(tree.Left);
                    var solveRight = Solve(tree.Right);

                    solveLeft.Variables.UnionWith(solveRight.Variables);
                    solveLeft.OMS.AddRange(solveRight.OMS);

                    var MergedVars = solveLeft.Variables;
                    var MergedOMS = solveLeft.OMS;

                    bool isSat = SetsAreEqual(solveLeft.Variables, solveRight.Variables) ? 
                        solveLeft.isSatisfiable | solveRight.isSatisfiable : true;

                    return new SolutionData
                    {
                        OMS = MergedOMS,
                        isSatisfiable = isSat,
                        Variables = MergedVars
                    };

                case GateType.NOT:

                    var SolveLeft = Solve(tree.Left);
                    SolveLeft.OMS = OperandMatch.XorOM(SolveLeft.OMS, true);
                    return SolveLeft;
            }

            throw new Exception("Impossible possibily!");
        }
    }
}

using SAT.Gate;

namespace SAT.Solver
{
    public class CSolver
    {

        public string? varName;
        static bool SetsAreEqual<T>(HashSet<T> set1, HashSet<T> set2)
        {
            if (set1.Count != set2.Count)
                return false;

            return set1.SetEquals(set2);
        }

        public bool isGateSuperSet(CGate a, CGate b) 
        {
            if (a.Type == GateType.NOT | b.Type == GateType.NOT)
            {
                CGate? sup = a.Type == GateType.NOT ? a.Left : b.Type == GateType.NOT ? b.Left : null;
                CGate other = sup == a.Left ? b : a;

                if (sup.Type == other.Type) return false;
                if (sup.Type == GateType.OR & other.Type == GateType.AND) return false;

                return true;
            }

            if (a.Type == b.Type) return true;
            if (a.Type == GateType.OR & b.Type == GateType.AND) return true;

            return false;
        }

        public Tuple<bool, HashSet<string>> Solve(CGate tree)
        {
            if (tree.VariableName != null) 
            { 
                HashSet<string> vars = [tree.VariableName];

                if (varName == tree.VariableName)
                {
                    return Tuple.Create(true, vars);
                }
                else 
                {
                    varName = tree.VariableName;
                    return Tuple.Create(true, vars);
                }
            }

            GateType gateType = tree.Type ?? GateType.NONE;

            switch (gateType) 
            {
                case GateType.AND:

                    var leftSol = Solve(tree.Left);
                    var rightSol = Solve(tree.Right);

                    bool isEqForTrue = SetsAreEqual(leftSol.Item2, rightSol.Item2) ? 
                        isGateSuperSet(tree.Left, tree.Right) : true;
               
                    leftSol.Item2.UnionWith(rightSol.Item2);
                    var mergedVars = leftSol.Item2;

                    return Tuple.Create(isEqForTrue, mergedVars);

                case GateType.OR:

                    var solveLeft = Solve(tree.Left);
                    var solveRight = Solve(tree.Right);

                    solveLeft.Item2.UnionWith(solveRight.Item2);
                    var MergedVars = solveLeft.Item2;

                    return Tuple.Create(solveLeft.Item1 | solveRight.Item1, MergedVars);

                case GateType.NOT:
                    return Solve(tree.Left);
            }

            // Just calming down the compiler
            throw new Exception("Impossible possibily!");
        }
    }
}

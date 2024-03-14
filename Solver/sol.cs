using SAT.OM;

namespace SAT.Solver
{
    public class SolutionData
    {
        public bool isSatisfiable;
        public HashSet<string> Variables = [];
        public List<EOperandMatch> OMS = [];
    }
}

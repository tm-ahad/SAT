using SAT.Req;

namespace SAT.Solver
{
    public class SolutionData
    {
        public bool isSatisfiable;
        public HashSet<string> Variables = [];
        public RequirementMap req = new RequirementMap();
        public bool isNot;
    }
}

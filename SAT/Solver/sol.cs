namespace SAT.Solver
{
    public class SolutionData
    {
        public required HashSet<string> Variables;
        public required ESaticfiction satisfiction;
        public required StrictRequirement mustSet;
        public required OptionalRequirement orSet;

        public static SolutionData NotSatisfiable()
        {
            return new SolutionData()
            {
                satisfiction = ESaticfiction.None,
                mustSet = new StrictRequirement(),
                orSet = new OptionalRequirement(),
                Variables = []
            };
        }

        public bool isSatisfiable()
        {
            return CSaticfiction.isSatisfiable(satisfiction);
        }
    }
}

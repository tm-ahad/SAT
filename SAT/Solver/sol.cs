namespace SAT.Solver
{
    public class SolutionData
    {
        public required HashSet<string> Variables;
        public required ESaticfiction satisfiction;
        public required MustSet mustSet;

        public static SolutionData NotSatisfiable()
        {
            return new SolutionData()
            {
                satisfiction = ESaticfiction.None,
                mustSet = new MustSet(),
                Variables = []
            };
        }

        public bool isSatisfiable()
        {
            return CSaticfiction.isSatisfiable(satisfiction);
        }
    }
}

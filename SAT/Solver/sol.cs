namespace SAT.Solver
{
    public class SolutionData
    {
        public required bool? Satisfiction { get; set; }
        public required GroupSet GroupSet { get; set; }

        public static SolutionData NotSatisfiable()
        {
            return new SolutionData()
            {
                Satisfiction = false,
                GroupSet = new()
            };
        }

        public bool? IsSatisfiable() => Satisfiction;
    }
}

namespace SAT.Solver
{
    public class SolutionData
    {
        public required bool? Satisfiction { get; set; }
        public required GroupSet GroupSet { get; set; }

        public bool? IsSatisfiable() => Satisfiction;
    }
}

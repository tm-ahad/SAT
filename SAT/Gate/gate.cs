namespace SAT.Gate
{
    public class CGate(GateType type)
    {
        public required string Literal { get; set; }
        public GateType Type { get; set; } = type;
        public string? Variable { get; set; }
        public CGate? Right { get; set; }
        public CGate? Left { get; set; }
        public int Variables { get; set; }
        public bool Root { get; set; }
    }
}

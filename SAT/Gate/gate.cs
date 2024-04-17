namespace SAT.Gate
{
    public class CGate
    {
        public required string Literal { get; set; }
        public string? Variable { get; set; }
        public GateType Type { get; set; }
        public CGate? Right { get; set; }
        public CGate? Left { get; set; }
        
        public CGate(GateType type)
        {
            Type = type;
        }
    }
}

namespace SAT.Gate
{
    public class CGate<T>
    {
        public GateType? Type { get; set; }
        public CGate<T>? Left { get; set; }
        public CGate<T>? Right { get; set; }
        public T? VariableName { get; set; }

        public CGate(GateType type)
        {
            Type = type;
        }
    }
}

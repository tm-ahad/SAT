using SAT.OM;
using System.Text;

namespace SAT.Gate
{
    public class CGate
    {
        public GateType? Type { get; set; }
        public CGate? Left { get; set; }
        public CGate? Right { get; set; }
        public string? VariableName { get; set; }

        public CGate(GateType type)
        {
            Type = type;
        }

        public List<EOperandMatch> OM()
        {
            switch (Type)
            {
                case GateType.OR: return [EOperandMatch.Same1, EOperandMatch.Diff];
                case GateType.AND: return [EOperandMatch.Same1];
                default: throw new Exception($"Cannot define om of gate {Type}");
            }
        }
    }
}

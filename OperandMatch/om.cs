using SAT.Gate;

namespace SAT.OM
{
    public enum EOperandMatch
    {
        Same0,
        Diff,
        Same1
    }

    public class OperandMatch
    {
        public static EOperandMatch FromNum(byte num) 
        {
            EOperandMatch res;

            if (Enum.TryParse(num.ToString(), out res))
            {
                return res;
            }

            throw new Exception($"Cannot convert number {num} to a OM");
        }

        public static byte ToNum(EOperandMatch om)
        {
            return (byte)om;
        }

        public static List<EOperandMatch> XorOM(List<EOperandMatch> om, bool b) 
        {
            List<EOperandMatch> allOm = [EOperandMatch.Same0, EOperandMatch.Diff, EOperandMatch.Same1];

            if (!b)
                return om;

            foreach (EOperandMatch m in om)
            {
                allOm.Remove(m);
            }

            return allOm;
        }
    }
}

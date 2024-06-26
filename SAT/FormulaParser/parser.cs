using SAT.Sanitizer;
using SAT.Gate;

namespace SAT.FormulaParser
{
    public class CFormulaParser
    {
        private static readonly Dictionary<char, GateType> OperatorMappings = new()
        {
            {'&', GateType.AND},
            {'|', GateType.OR},
            {'!', GateType.NOT}
        };

        public static GateType? TryGetOperator(char op)
        {
            if (OperatorMappings.TryGetValue(op, out GateType value)) return value;
            return null;
        }

        public static CGate Parse(string? formula)
        {
            if (string.IsNullOrEmpty(formula)) throw new Exception("Empty formula is not allowed");
            formula = CSanitizer.Sanitize(formula);

            Tuple<CGate, int> ret = ParseFormula(formula);
            CGate final = ret.Item1;
            int variables = ret.Item2;

            final.Variables = variables;
            final.Root = true;

            return final;
        }

        private static Tuple<CGate, int> ParseFormula(string formula)
        {
            List<GateType> operators = [];

            HashSet<char> variables = [];
            List<CGate> gates = [];

            int i = 0;

            while (i < formula.Length)
            {
                char c = formula[i];
                GateType? op = TryGetOperator(c);

                if (char.IsLetter(c))
                {
                    gates.Add(new CGate(GateType.VARIABLE) { Variable = c });
                    variables.Add(c);
                }
                else if (op != null)
                {
                    operators.Add((GateType)op);
                }
                else if (c == '(')
                {
                    int subStart = i + 1;
                    int subEnd = FindClosingParenthesis(formula, subStart);

                    string subformula = formula.Substring(subStart, subEnd - subStart);
                    gates.Add(ParseFormula(subformula).Item1);

                    i = subEnd;
                }
                i++;
            }

            if (gates.Count == 0)
                throw new Exception("No gates found in the formula");

            CGate final = BuildGateStructure(gates, operators);
            return Tuple.Create(final, variables.Count);
        }

        private static int FindClosingParenthesis(string formula, int start)
        {
            int depth = 1;
            for (int i = start; i < formula.Length; i++)
            {
                if (formula[i] == '(') depth++;
                else if (formula[i] == ')') depth--;
                if (depth == 0) return i;
            }
            throw new Exception("Unbalanced parentheses in formula");
        }

        private static CGate BuildGateStructure(List<CGate> gates, List<GateType> operators)
        {
            CGate final = null;

            for (int i = 0; i < operators.Count; i++)
            {
                if (final != null)
                {
                    CGate? leftPart = i + 1 < gates.Count ? gates[i] : null;
                    final = new CGate(operators[i]) { Left = leftPart, Right = final };
                }
                else
                {
                    CGate? rightPart = 1 < gates.Count ? gates[1] : null;
                    final = new CGate(operators[i]) { Left = gates[0], Right = rightPart };
                }
            }

            if (final == null)
            {
                return gates[0];
            }

            return final;
        }
    }
}

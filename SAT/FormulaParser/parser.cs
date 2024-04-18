using SAT.Gate;
using System.Text.RegularExpressions;

namespace SAT.FormulaParser
{
    public class CFormulaParser
    {
        public const char OPEN_BRAC = ')';
        public const char CLOSE_BRAC = '(';

        private static readonly Dictionary<char, GateType> OperatorMappings = new Dictionary<char, GateType>
        {
            {'&', GateType.AND},
            {'|', GateType.OR},
            {'!', GateType.NOT}
        };

        public static GateType FromOperator(char op)
        {
            if (OperatorMappings.ContainsKey(op)) return OperatorMappings[op];
            throw new ArgumentException($"Invalid boolean operator '{op}'");
        }

        public static CGate Parse
            (
                string formula, 
                string pattern = @"\((?>[^()]+|\((?<Depth>)|\)(?<-Depth>))*(?(Depth)(?!))\)",
                bool isFirst = true
            )
        {
            if (string.IsNullOrEmpty(formula)) throw new Exception("Empty formula is not allowed");
            formula = formula.Replace(" ", "");

            Regex reg = new Regex(pattern);
            MatchCollection match = reg.Matches(formula);

            formula = Regex.Replace(formula, pattern, "");

            if (match.Count == 0)
            {
                string alphabetRegex = @"[A-Za-z]";
                return Parse(formula, alphabetRegex, false);
            }
            else 
            {
                List<CGate> gateList = [];

                foreach (Match m in match)
                {
                    if (isFirst)
                    {
                        string brac = m.Value;
                        int end = brac.Count() - 1;

                        brac = brac[1..end];
                        gateList.Add(Parse(brac));
                    }
                    else 
                    {
                        gateList.Add(new CGate(GateType.VARIABLE)
                        {
                            Variable = m.Value,
                            Literal = m.Value
                        });
                    }
                }

                var list = formula.Where(c => c == '!');

                for (int i = 0; i < list.Count(); i++) 
                {
                    gateList[i] = new CGate(GateType.NOT)
                    {
                        Literal = $"!{gateList[i].Literal}",
                        Left = gateList[i],
                    };
                }

                formula = formula.Replace("!", "");

                for (int i = 0; i < formula.Count(); i++)
                {
                    char op = formula[i];

                    CGate left = gateList[i];
                    CGate right = gateList[i+1];

                    gateList[i] = new CGate(FromOperator(op))
                    {
                        Literal = $"{left.Literal}{op}{right.Literal}",
                        Right = right,
                        Left = left
                    };
                }

                return gateList.First();
            }
        }
    }
}

using SAT.Gate;
using System.Text.RegularExpressions;

namespace SAT.FormulaParser
{
    public class FormulaParser
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
            if (OperatorMappings.ContainsKey(op))
                return OperatorMappings[op];

            throw new ArgumentException($"Invalid boolean operator '{op}'");
        }

        public static CGate ParseNotBrace(string formula)
        {
            string pat = @"(?=[!&|])";
            string[] split = Regex.Split(formula, pat);

            string first = split.First();
            char firstOp = first[0];

            if (firstOp == '!')
            {
                CGate varGate = new CGate(GateType.VARIABLE)
                {
                    VariableName = first[1..]
                };

                return new CGate(GateType.NOT)
                {
                    Left = varGate
                };
            }
            else if (char.IsLetter(firstOp))
            {
                string firstName = firstOp.ToString();

                CGate tree = new CGate(GateType.VARIABLE)
                {
                    VariableName = firstName
                };

                foreach (string part in split[1..])
                {
                    char oper = part[0];
                    string varName = part[1..];
                    CGate right = new CGate(GateType.VARIABLE) 
                    { 
                        VariableName = varName 
                    };

                    tree = new CGate(FromOperator(oper))
                    {
                        Left = tree,
                        Right = right
                    };
                }

                return tree;
            }

            throw new Exception("Invalid formula");
        }

        public static CGate Parse(string formula)
        {
            if (string.IsNullOrEmpty(formula))
                throw new Exception("Empty formula is not allowed");

            formula = formula.Replace(" ", "");
            string pattern = @"\((?>[^()]+|\((?<Depth>)|\)(?<-Depth>))*(?(Depth)(?!))\)";

            Regex reg = new Regex(pattern);
            MatchCollection match = reg.Matches(formula);

            formula = Regex.Replace(formula, pattern, "");

            if (match.Count == 0)
            {
                return ParseNotBrace(formula);
            }
            else 
            {
                List<CGate> gateList = [];

                foreach (Match m in match)
                {
                    string brac = m.ToString();
                    int end = brac.Count() - 1;
                    
                    brac = brac[1..end];
                    gateList.Add(Parse(brac));
                }

                var list = formula.Where(c => c == '!');

                for (int i = 0; i < list.Count(); i++) 
                {
                    gateList[i] = new CGate(GateType.NOT)
                    {
                        Left = gateList[i]
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
                        Left = left,
                        Right = right
                    };
                }

                return gateList.First();
            }
        }
    }
}

using SAT.Gate;
using System.Text.RegularExpressions;

namespace SAT.FormulaParser
{
    using Tree = CGate<string>;

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
            if (OperatorMappings.ContainsKey(op))
                return OperatorMappings[op];

            throw new ArgumentException($"Invalid boolean operator '{op}'");
        }

        public static Tree ParseNotBrace(string formula)
        {
            if (formula.StartsWith("!"))
            {
                Tree varGate = new Tree(GateType.VARIABLE)
                {
                    VariableName = formula[1..]
                };

                return new Tree(GateType.NOT)
                {
                    Left = varGate
                };
            }

            string pat = @"(?=[!&|])";
            string[] split = Regex.Split(formula, pat);

            if (formula.Length == 1 & char.IsLetter(formula[0]))
                return new Tree(GateType.VARIABLE)
                {
                    VariableName = formula,
                };

            string first = split.First();
            char firstOp = first[0];

            
            if (char.IsLetter(firstOp))
            {
                string firstName = firstOp.ToString();

                Tree tree = new Tree(GateType.VARIABLE)
                {
                    VariableName = firstName
                };

                foreach (string part in split[1..])
                {
                    char oper = part[0];
                    string varName = part[1..];
                    Tree right = new Tree(GateType.VARIABLE) 
                    { 
                        VariableName = varName 
                    };

                    tree = new Tree(FromOperator(oper))
                    {
                        Left = tree,
                        Right = right
                    };
                }

                return tree;
            }

            throw new Exception("Invalid formula");
        }

        public static Tree Parse
            (
                string formula, 
                string pattern = @"\((?>[^()]+|\((?<Depth>)|\)(?<-Depth>))*(?(Depth)(?!))\)",
                bool isFirst = true
            )
        {
            if (string.IsNullOrEmpty(formula))
                throw new Exception("Empty formula is not allowed");

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
                List<Tree> gateList = [];

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
                        gateList.Add(new Tree(GateType.VARIABLE)
                        {
                            VariableName = m.Value,
                        });
                    }
                }

                var list = formula.Where(c => c == '!');

                for (int i = 0; i < list.Count(); i++) 
                {
                    gateList[i] = new Tree(GateType.NOT)
                    {
                        Left = gateList[i]
                    };
                }

                formula = formula.Replace("!", "");

                for (int i = 0; i < formula.Count(); i++)
                {
                    char op = formula[i];

                    Tree left = gateList[i];
                    Tree right = gateList[i+1];

                    gateList[i] = new Tree(FromOperator(op))
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

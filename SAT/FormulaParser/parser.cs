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

        public static GateType FromOperator(char op)
        {
            if (OperatorMappings.TryGetValue(op, out GateType value)) return value;
            throw new ArgumentException($"Invalid boolean operator '{op}'");
        }

        public static CGate Parse(string? formula)
        {
            if (string.IsNullOrEmpty(formula)) throw new Exception("Empty formula is not allowed");
            formula = CSanitizer.Sanitize(formula);

            return ParseFormula(formula);
        }

        private static CGate ParseFormula(string formula)
        {
            HashSet<char> variables = new();
            Stack<char> operators = new();
            Stack<int> parenStack = new();
            Stack<CGate> gates = new();

            for (int i = 0; i < formula.Length; i++)
            {
                char ch = formula[i];

                if (char.IsWhiteSpace(ch))
                {
                    continue;
                }
                else if (ch == '(')
                {
                    parenStack.Push(i);
                }
                else if (ch == ')')
                {
                    if (parenStack.Count == 0)
                    {
                        throw new ArgumentException($"Mismatched parentheses at position {i}");
                    }

                    int start = parenStack.Pop();
                    string subformula = formula.Substring(start + 1, i - start - 1);
                    CGate gate = ParseFormula(subformula);
                    gates.Push(gate);
                }
                else if (OperatorMappings.ContainsKey(ch))
                {
                    if (ch == '!')
                    {
                        ParseNotOperator(formula, ref i, gates);
                    }
                    else
                    {
                        while (operators.Count > 0 && Priority(operators.Peek()) >= Priority(ch))
                        {
                            CreateGateFromStacks(operators, gates);
                        }
                        operators.Push(ch);
                    }
                }
                else if (char.IsLetter(ch))
                {
                    variables.Add(ch);
                    gates.Push(new CGate(GateType.VARIABLE)
                    {
                        Variable = ch.ToString(),
                        Literal = ch.ToString()
                    });
                }
                else
                {
                    throw new ArgumentException($"Unexpected character '{ch}' in formula at position {i}");
                }
            }

            while (operators.Count > 0)
            {
                CreateGateFromStacks(operators, gates);
            }

            if (gates.Count != 1)
            {
                throw new Exception("Error parsing formula");
            }

            CGate tree = gates.Pop();
            tree.Variables = variables.Count;
            tree.Root = true;

            return tree;
        }

        private static void CreateGateFromStacks(Stack<char> operators, Stack<CGate> gates)
        {
            char op = operators.Pop();
            CGate right = gates.Pop();
            CGate left = gates.Pop();

            gates.Push(new CGate(FromOperator(op))
            {
                Literal = $"{left.Literal}{op}{right.Literal}",
                Left = left,
                Right = right
            });
        }

        private static void ParseNotOperator(string formula, ref int i, Stack<CGate> gates)
        {
            if (i + 1 < formula.Length && formula[i + 1] == '(')
            {
                int end = FindMatchingParenthesis(formula, i + 1);
                string subformula = formula.Substring(i + 2, end - i - 2);
                CGate gate = ParseFormula(subformula);
                gates.Push(new CGate(GateType.NOT)
                {
                    Literal = $"!{gate.Literal}",
                    Left = gate
                });
                i = end;
            }
            else
            {
                if (i + 1 >= formula.Length || !char.IsLetter(formula[i + 1]))
                {
                    throw new ArgumentException($"Invalid syntax after NOT operator at position {i}");
                }

                i++;
                gates.Push(new CGate(GateType.NOT)
                {
                    Literal = $"!{formula[i]}",
                    Left = new CGate(GateType.VARIABLE)
                    {
                        Variable = formula[i].ToString(),
                        Literal = formula[i].ToString()
                    }
                });
            }
        }

        private static int FindMatchingParenthesis(string formula, int startIndex)
        {
            int depth = 1;
            for (int i = startIndex + 1; i < formula.Length; i++)
            {
                if (formula[i] == '(') depth++;
                if (formula[i] == ')') depth--;
                if (depth == 0) return i;
            }
            throw new ArgumentException("No matching closing parenthesis found");
        }

        private static int Priority(char op)
        {
            return op switch
            {
                '|' => 1,
                '&' => 2,
                '!' => 3,
                _ => throw new ArgumentException($"Invalid operator '{op}'")
            };
        }
    }
}

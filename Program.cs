using SAT.FormulaParser;
using SAT.Solver;
using SAT.Gate;

namespace SAT
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("POLYNOMIAL solution for satisfibility (A.K.A SAT) problem\n");
                Console.WriteLine($"Usage: {args[0]} <formula>");
                Console.WriteLine("Operators - ");
                Console.WriteLine("| is used as the OR operator.");
                Console.WriteLine("& is used as the AND operator.");
                Console.WriteLine("! is used as the NOT operator.");
                Console.WriteLine("( and ) is used as brackets.");
                return;
            }

            string formula = args[1];

            CGate tree = CFormulaParser.Parse(formula);
            CSolver solver = new CSolver();

            bool res = solver.Solve(tree).Item1;

            if (res)
            {
                Console.WriteLine($"formula \"{formula}\" is satisfiable.");
                return;
            }

            Console.WriteLine($"formula \"{formula}\" is not satisfiable.");
        }
    }
}

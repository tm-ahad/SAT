using SAT.FormulaParser;
using SAT.Solver;
using SAT.Gate;

namespace SAT
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("POLYNOMIAL solution for satisfibility (A.K.A SAT) problem\n");
                Console.WriteLine($"Usage: SAT.exe <formula>");
                Console.WriteLine("Operators - ");
                Console.WriteLine("| is used as the OR operator.");
                Console.WriteLine("& is used as the AND operator.");
                Console.WriteLine("! is used as the NOT operator.");
                Console.WriteLine("( and ) is used as brackets.");
                return;
            }

            string formula = args[0];

            CGate tree = CFormulaParser.Parse(formula);
            bool res = CSolver.Solve(tree).isSatisfiable();

            if (res)
            {
                Console.WriteLine($"formula \"{formula}\" is satisfiable.");
                return;
            }

            Console.WriteLine($"formula \"{formula}\" is not satisfiable.");
        }
    }
}

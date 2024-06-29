using SAT.FormulaParser;
using System.Diagnostics;
using SAT.Solver;
using SAT.Gate;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0].ToLower() == "help")
        {
            Console.WriteLine("Polynomial solution for satisfibility (A.K.A SAT) problem");
            Console.WriteLine("Operators - ");
            Console.WriteLine("| is used as the OR operator.");
            Console.WriteLine("& is used as the AND operator.");
            Console.WriteLine("! is used as the NOT operator.");
            Console.WriteLine("( and ) is used as brackets.");
            return;
        }

        string? formula = args[0];
        Stopwatch sw = Stopwatch.StartNew();

        CGate tree = CFormulaParser.Parse(formula);
        CSolver solver = new();

        bool? sat = solver.Solve(tree).IsSatisfiable();
        sw.Stop();

        Console.WriteLine(sat == true ? "satisfiable" : "not satisfiable");
        Console.WriteLine($"{(float)sw.ElapsedTicks / 10}microseconds");
    }
}
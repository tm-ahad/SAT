using SAT.FormulaParser;
using System.Diagnostics;
using SAT.Solver;
using SAT.Gate;

Console.WriteLine("Polynomial solution for satisfibility (A.K.A SAT) problem");
Console.WriteLine("Enter 'exit' or '1' to exit the program");
Console.WriteLine("Operators - ");
Console.WriteLine("| is used as the OR operator.");
Console.WriteLine("& is used as the AND operator.");
Console.WriteLine("! is used as the NOT operator.");
Console.WriteLine("( and ) is used as brackets.");

while (true)
{
    Console.Write(">> ");

    string? formula = Console.ReadLine();
    if (formula.ToLower() == "exit" || formula == "1")
    {
        Environment.Exit(0);
    }

    Stopwatch sw = Stopwatch.StartNew();

    CGate tree = CFormulaParser.Parse(formula);
    tree.Print();
    CSolver solver = new(tree.Variables);

    bool? sat = solver.Solve(tree).IsSatisfiable();
    sw.Stop();

    Console.WriteLine(sat == true ? "satisfiable" : "not satisfiable");
    Console.WriteLine($"{sw.ElapsedTicks / 10}μs");
}

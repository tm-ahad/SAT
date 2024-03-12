using SAT.FormulaParser;
using SAT.Gate;
using SAT.Solver;

Console.WriteLine("POLYNOMIAL solution for satisfibility (A.K.A SAT) problem\n");
Console.Write("Formula: ");

string formula = Console.ReadLine();

CGate tree = FormulaParser.Parse(formula);
Solver solver = new Solver();

bool res = solver.Solve(tree).Item1;

if (res)
{
    Console.WriteLine($"formula \"{formula}\" is satisfiable.");
    return;
}

Console.WriteLine($"formula \"{formula}\" is not satisfiable.");

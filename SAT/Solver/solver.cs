﻿using SAT.Gate;

namespace SAT.Solver
{
    public class CSolver
    {
        public static string InvertS(string s) 
        {
            if (s.StartsWith('!'))
            {
                return s[1..];
            }
            else
            {
                return $"!{s}";
            }
        }

        public SolutionData Solve(CGate tree)
        {
            if (tree.Variable != null)
            {
                GroupSet gr = new(tree.Variable.ToString());

                return new SolutionData()
                {
                    Satisfiction = true,
                    GroupSet = gr
                };
            }

            SolutionData sol;

            switch (tree.Type)
            {
                case GateType.AND:
                    SolutionData leftSol = Solve(tree.Left);
                    SolutionData rightSol = Solve(tree.Right);

                    leftSol.GroupSet.And(rightSol.GroupSet);

                    sol = new SolutionData()
                    {
                        Satisfiction = tree.Root ? leftSol.GroupSet.IsSatisfiable() : null,
                        GroupSet = leftSol.GroupSet,
                    };
                    break;

                case GateType.OR:

                    SolutionData leftSol_ = Solve(tree.Left);
                    SolutionData rightSol_ = Solve(tree.Right);

                    if (tree.Root)
                    {
                        return new SolutionData()
                        {
                            Satisfiction = leftSol_.IsSatisfiable() || rightSol_.IsSatisfiable(),
                            GroupSet = new GroupSet("")
                        };
                    }

                    leftSol_.GroupSet.Or(rightSol_.GroupSet);

                    sol = new SolutionData()
                    {
                        Satisfiction = tree.Root ? leftSol_.GroupSet.IsSatisfiable() : null,
                        GroupSet = leftSol_.GroupSet,
                    };
                    break;

                case GateType.NOT:

                    SolutionData SolveLeft = Solve(tree.Left);
                    SolveLeft.GroupSet.Not();

                    sol = new SolutionData()
                    {
                        Satisfiction = tree.Root ? SolveLeft.GroupSet.IsSatisfiable() : null,
                        GroupSet = SolveLeft.GroupSet,
                    };

                    break;

                default:
                    throw new Exception("Invalid gate type");
            }

            if (tree.Root)
            {
                sol.Satisfiction = sol.IsSatisfiable();
            }

            return sol;
        }
    }
}

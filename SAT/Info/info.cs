namespace SAT.Info
{
    public static class Info
    {
        public static void Print()
        {
            Console.WriteLine("Polynomial solution for satisfibility (A.K.A SAT) problem");
            Console.WriteLine("Operators - ");
            Console.WriteLine("| is used as the OR operator.");
            Console.WriteLine("& is used as the AND operator.");
            Console.WriteLine("! is used as the NOT operator.");
            Console.WriteLine("( and ) is used as brackets.");
            Console.WriteLine("Note: write expression '!v' as '(!v)' for every variable");
        }
    }
}

using System.Text;

namespace SAT.Gate
{
    public class CGate
    {
        public GateType? Type { get; set; }
        public CGate? Left { get; set; }
        public CGate? Right { get; set; }
        public string? VariableName { get; set; }

        public CGate(GateType type)
        {
            Type = type;
        }

        public void PrintCGateTree(CGate node, int depth = 0)
        {
            if (node == null)
                return;

            // Indent based on depth
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < depth; i++)
            {
                sb.Append("  "); // Adjust spacing as needed
            }

            // Print current node with gate type
            sb.Append($"[{node.Type}] ");
            if (node.VariableName != null && node.VariableName != "")
            {
                sb.Append(": ");
                sb.Append(node.VariableName);
            }
            Console.WriteLine(sb.ToString());

            // Recursively print left and right subtrees
            PrintCGateTree(node.Left, depth + 1);
            PrintCGateTree(node.Right, depth + 1);
        }
    }
}

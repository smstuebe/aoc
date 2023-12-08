public class Day8 : IDay
{
    private string[] lines;

    public Day8()
    {
        lines = File.ReadAllLines("day8/input.txt");
        Instructions = lines[0];
        Nodes = lines.Skip(2).Select(l => (name: l[..3], left: l[7..10], right: l[12..15]))
            .ToDictionary(l => l.name, l => l);
    }

    public Dictionary<string, (string name, string left, string right)> Nodes { get; set; }

    public string Instructions { get; set; }

    public void FirstPart()
    {
        int i = 0;
        var currentNode = "AAA";
        while (currentNode != "ZZZ" || i % Instructions.Length != 0)
        {
            var dir = Instructions[i % Instructions.Length];
            currentNode = dir == 'R' ? Nodes[currentNode].right : Nodes[currentNode].left;
            ++i;
        }

        Console.WriteLine(i);
    }

    public void SecondPart()
    {
        var startNodes = Nodes.Keys.Where(n => n.EndsWith("A")).ToArray();

        int GetSteps(string currentNode)
        {
            int i = 0;
            while (i % Instructions.Length != 0 || currentNode[2] != 'Z')
            {
                var dir = Instructions[i % Instructions.Length];
                currentNode = dir == 'R' ? Nodes[currentNode].right : Nodes[currentNode].left;

                ++i;
            }

            return i;
        }

        static long GCD(long n1, long n2)
        {
            if (n2 == 0)
                return n1;
            return GCD(n2, n1 % n2);
        }

        var steps = startNodes.Select(x => (long)GetSteps(x)).ToArray();
        var neededSteps = steps.Aggregate((S, val) => S * val / GCD(S, val));

        Console.WriteLine(neededSteps);
    }
}
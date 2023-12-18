using System.Text;
using System.Text.RegularExpressions;
using P = (int x, int y);

public class Day18 : IDay
{
    private string[] lines;

    public Day18()
    {
        lines = File.ReadAllLines("day18/input.txt");
    }

    class Command
    {
        public string Direction { get; set; }
        public int Distance { get; set; }
        public string Color { get; set; }

        public static Command Parse(string cmd)
        {
            var m = Regex.Match(cmd, @"^(?<dir>\w) (?<dist>\d+).*(?<col>#[a-f0-9]{6})");
            return new Command
            {
                Direction = m.Groups["dir"].Value,
                Distance = int.Parse(m.Groups["dist"].Value),
                Color = m.Groups["col"].Value
            };
        }
    }

    public void FirstPart()
    {
        var currentPos = new P(250, 250);
        var plan = new int[500, 500];
        var cmds = lines.Select(Command.Parse).ToList();

        foreach (var cmd in cmds)
        {
            for (int i = 1; i <= cmd.Distance; i++)
            {
                currentPos = cmd.Direction switch
                {
                    "R" => (currentPos.x + 1, currentPos.y),
                    "L" => (currentPos.x - 1, currentPos.y),
                    "D" => (currentPos.x, currentPos.y + 1),
                    "U" => (currentPos.x, currentPos.y - 1)
                };

                plan[currentPos.y, currentPos.x] = 1;
            }
        }

        var toFill = new Stack<P>();
        toFill.Push(new P(250, 277));
        var w = plan.GetUpperBound(1);
        var h = plan.GetUpperBound(0);
        while (toFill.Any())
        {
            var c = toFill.Pop();
            if (c.x < w && c.x >= 0 && c.y >= 0 && c.y < h && plan[c.y, c.x] == 0)
            {
                plan[c.y, c.x] = 1;
                toFill.Push((c.x + 1, c.y));
                toFill.Push((c.x - 1, c.y));
                toFill.Push((c.x, c.y + 1));
                toFill.Push((c.x, c.y - 1));
            }
        }

        var b = new StringBuilder(500 * 503);
        for (int y = 0; y < plan.GetUpperBound(0); y++)
        {
            for (int x = 0; x < plan.GetUpperBound(1); x++)
            {
                b.Append(plan[y, x] > 0 ? '#' : ' ');
            }

            b.AppendLine();
        }

        File.WriteAllText("test.txt", b.ToString());

        var area = 0;
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                if (plan[y, x] == 1)
                {
                    area++;
                }
            }
        }

        Console.WriteLine(area);
    }

    public void SecondPart()
    {
        //Console.WriteLine(sum);
    }
}
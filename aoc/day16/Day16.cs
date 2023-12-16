public class Day16 : IDay
{
    private string[] lines;

    public Day16()
    {
        lines = File.ReadAllLines("day16/input.txt");

    }

    public record Tile
    {
        public int Visited { get; set; }
        public List<(int dx, int dy)> Dirs { get; init; } = new();
    }

    public void FirstPart()
    {
        Console.WriteLine(GetEnergy((0, 0, 1, 0)));
    }

    public void SecondPart()
    {
        var h = lines.Length;
        var w = lines[0].Length;

        var possibilities = new[]
        {
            Enumerable.Range(0, w).Select(x => (x, 0, 0, 1)), 
            Enumerable.Range(0, w).Select(x => (x, h - 1, 0, -1)),
            Enumerable.Range(0, h).Select(y => (0, y, 1, 0)),
            Enumerable.Range(0, h).Select(y => (w - 1, y, -1, 0))
        }.SelectMany(t => t);

        var max = possibilities.Select(GetEnergy).Max();
        Console.WriteLine(max);
    }


    int GetEnergy((int x, int y, int dx, int dy) start)
    {
        var visited = lines.Select(l => l.Select(_ => new Tile()).ToArray()).ToArray();
        var h = lines.Length;
        var w = lines[0].Length;
        var toVisit = new Stack<(int x, int y, int dx, int dy)>();
        toVisit.Push(start);

        while (toVisit.Any())
        {
            var t = toVisit.Pop();
            if (t.x < 0 || t.x >= w || t.y < 0 || t.y >= h)
                continue;

            if (visited[t.y][t.x].Dirs.Any(d => d == (t.dx, t.dy)))
                continue;

            visited[t.y][t.x].Visited++;
            visited[t.y][t.x].Dirs.Add((t.dx, t.dy));

            switch (lines[t.y][t.x])
            {
                case '.':
                    toVisit.Push((t.x + t.dx, t.y + t.dy, t.dx, t.dy));
                    break;

                case '/':
                    if (t.dy != 0)
                        toVisit.Push((t.x - t.dy, t.y, -t.dy, 0));
                    else
                        toVisit.Push((t.x, t.y - t.dx, 0, -t.dx));
                    break;
                case '\\':
                    if (t.dy != 0)
                        toVisit.Push((t.x + t.dy, t.y, t.dy, 0));
                    else
                        toVisit.Push((t.x, t.y + t.dx, 0, t.dx));
                    break;
                case '-':
                    if (t.dy != 0)
                    {
                        toVisit.Push((t.x + 1, t.y, 1, 0));
                        toVisit.Push((t.x - 1, t.y, -1, 0));
                    }
                    else
                        toVisit.Push((t.x + t.dx, t.y + t.dy, t.dx, t.dy));
                    break;
                case '|':
                    if (t.dx != 0)
                    {
                        toVisit.Push((t.x, t.y + 1, 0, 1));
                        toVisit.Push((t.x, t.y - 1, 0, -1));
                    }
                    else
                        toVisit.Push((t.x + t.dx, t.y + t.dy, t.dx, t.dy));
                    break;
            }
        }

        return visited.SelectMany(v => v.Select(w => w.Visited > 0)).Count(s => s);
    }
}
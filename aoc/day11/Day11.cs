public class Day11 : IDay
{
    private List<string> lines;
    private List<(int x, int y)> _galaxies;
    private List<int> _emptyColumns;
    private List<int> _emptyRows;

    public Day11()
    {
        lines = File.ReadAllLines("day11/input.txt").ToList();

        _emptyRows = lines.Select((l, i) => (l, i))
            .Where(x => x.l.All(l => l == '.'))
            .Select(x => x.i).ToList();

        _emptyColumns = new List<int>();
        for (int i = 0; i < lines[0].Length; i++)
        {
            if (lines.All(l => l[i] == '.'))
            {
                _emptyColumns.Add(i);
            }
        }
        
        _galaxies = lines.SelectMany(
                (l, y) => l.Select((c, x) => (c, x, y))
                    .Where(x => x.c == '#')
                    .Select(x => (x.x, x.y)))
            .ToList();
    }

    public void FirstPart()
    {
        Console.WriteLine(DistanceAll(2));
    }

    public void SecondPart()
    {
        Console.WriteLine(DistanceAll(1000000));
    }

    long DistanceAll(int expansion)
    {
        var sum = 0L;
        for (int g = 0; g < _galaxies.Count; g++)
        {
            for (int g2 = g+1; g2 < _galaxies.Count; g2++)
            {
                sum += Distance(_galaxies[g], _galaxies[g2], expansion);
            }
        }
        return sum;
    }
    
    long Distance((int x, int y) g1, (int x, int y) g2, int expansion)
    {
        var dx = g2.x - g1.x;
        var dy = g2.y - g1.y;
        var minX = Math.Min(g2.x, g1.x);
        var maxX = Math.Max(g2.x, g1.x);
        var minY = Math.Min(g2.y, g1.y);
        var maxY = Math.Max(g2.y, g1.y);

        var rows = _emptyRows.Count(r => r > minY && r < maxY);
        var columns = _emptyColumns.Count(r => r > minX && r < maxX);

        return Math.Abs(dx) + Math.Abs(dy) + (rows + columns) * (expansion-1);
    }
}
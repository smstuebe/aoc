using P = (int x, int y);

public class Day21 : IDay
{
    private string[] lines;

    public Day21()
    {
        lines = File.ReadAllLines("day21/input.txt");
    }

    public void FirstPart()
    {
        var start = lines.Select((l, y) => (x: l.IndexOf('S'), y)).First(p => p.x != -1);
        lines[start.y] = lines[start.y].Replace('S', '.');
        var tiles = new HashSet<P> { start };
        var w = lines[0].Length;
        var h = lines.Length;

        for (int i = 0; i < 64; i++)
        {
            var t = new HashSet<P>();

            foreach (var tt in tiles)
            {
                if (tt.y > 0 && lines[tt.y -1][tt.x] == '.')
                    t.Add((tt.x, tt.y - 1));
                if (tt.x > 0 && lines[tt.y][tt.x-1] == '.')
                    t.Add((tt.x-1, tt.y));
                if (tt.y < h-1 && lines[tt.y + 1][tt.x] == '.')
                    t.Add((tt.x, tt.y + 1));
                if (tt.x < w-1 && lines[tt.y][tt.x+1] == '.')
                    t.Add((tt.x+1, tt.y));
            }

            tiles = t;
        }

        Console.WriteLine(tiles.Count);
    }


    public void SecondPart()
    {
    }
}
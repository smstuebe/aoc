using System.Text.RegularExpressions;
using P = (int x, int y, int z);

public class Day22 : IDay
{
    private string[] lines;

    public Day22()
    {
        lines = File.ReadAllLines("day22/input.txt");
    }

    public void FirstPart()
    {
        var bricks = lines.Select(l => Regex.Matches(l, @"(\d+)").Select(c => int.Parse(c.ValueSpan)).ToArray())
            .Select(p => (p1: new P(p[0], p[1], p[2]), p2: new P(p[3], p[4], p[5])))
            .OrderBy(b => Math.Min(b.p1.z, b.p2.z))
            .ToList();

        //Console.WriteLine(sum);
    }

    public void SecondPart()
    {
        //Console.WriteLine(sum);
    }
}
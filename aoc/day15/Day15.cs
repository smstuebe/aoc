using System.Text.RegularExpressions;

public class Day15 : IDay
{
    private string[] lines;

    public Day15()
    {
        lines = File.ReadAllLines("day15/input.txt");
    }

    public void FirstPart()
    {
        var sum = lines[0].Split(',')
            .Select(s => s.Aggregate((byte)0, (a, c) => (byte)((a + (byte)c) * 17)))
            .Sum(x => x);
        Console.WriteLine(sum);
    }

    public void SecondPart()
    {
        var instructions = lines[0].Split(',');
        var boxes = Enumerable.Range(0, 256)
            .Select(x => new List<(string label, int focus)>())
            .ToList();

        foreach (var instruction in instructions)
        {
            var m = Regex.Match(instruction, @"^(?<l>.*)(?<op>[=-])(?<f>\d*)$");

            var box = m.Groups["l"].Value.Aggregate((byte)0, (a, c) => (byte)((a + (byte)c) * 17));
            var lens = boxes[box].FirstOrDefault(l => l.label == m.Groups["l"].Value);
            if (m.Groups["op"].Value == "-")
                boxes[box].Remove(lens);
            else
            {
                var f = int.Parse(m.Groups["f"].ValueSpan);
                var newLens = (m.Groups["l"].Value, f);

                if (lens.label is null)
                    boxes[box].Add(newLens);
                else
                {
                    var i = boxes[box].IndexOf(lens);
                    boxes[box].RemoveAt(i);
                    boxes[box].Insert(i, newLens);
                }
            }
        }

        var sum = boxes
            .SelectMany((b, bi) => b.Select((l, li) => (bi + 1) * (li + 1) * l.focus))
            .Sum();

        Console.WriteLine(sum);
    }
}
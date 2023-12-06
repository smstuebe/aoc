public class Day6 : IDay
{
    private string[] lines;

    public Day6()
    {
        lines = File.ReadAllLines("day6/input.txt");
        var times = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Skip(1).Select(long.Parse);
        var maxDistance = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Skip(1).Select(long.Parse);
        Races = times.Zip(maxDistance).ToArray();

    }

    public (long time, long distance)[] Races { get; set; }

    long FindPossibilities((long time, long distance) race)
    {
        var possibilities = 0;
        for (long t = 0; t < race.time; t++)
        {
            var distance = t * (race.time - t);
            if (distance > race.distance)
            {
                possibilities++;
            }
        }

        return possibilities;
    }

    public void FirstPart()
    {
        var m = Races.Select(FindPossibilities).Aggregate(1L, (a, b) => a * b);
        Console.WriteLine(m);
    }

    public void SecondPart()
    {
        var time = lines[0].Replace(" ", "").Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Skip(1).Select(long.Parse).First();
        var distance = lines[1].Replace(" ", "").Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Skip(1).Select(long.Parse).First();

        var m = FindPossibilities((time, distance));
        Console.WriteLine(m);
    }
}
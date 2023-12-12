using System.Collections.Concurrent;

public class Day23 : IDay
{
    private string[] lines;
    private readonly ConcurrentDictionary<string, long> cache;

    public Day23()
    {
        lines = File.ReadAllLines("day12/input.txt");
        cache = new ConcurrentDictionary<string, long>();
    }

    public void FirstPart()
    {
        var pos = lines
            .Select(l => l.Split(" "))
            .Select(c => (c: c[0],
                control: c[1].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).ToArray()))
            .Select(p => GetPossibilities(p.c, p.control))
            .ToList();

        var sum = pos.Sum();
        Console.WriteLine(sum);
    }

    public void SecondPart()
    {
        var toCheck = lines
            .Select(l => l.Split(" "))
            .Select(c => (c: c[0],
                control: c[1].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).ToArray()))
            .Select(Expand)
            .Select(c => new Check(c.c, c.control))
            .ToList();

        Parallel.For(0, toCheck.Count,
            (int i) =>
            {
                var check = toCheck[i];
                check.Possibilities = GetPossibilities(check.Value, check.Groups);
            });

        var sum = toCheck.Sum(c => c.Possibilities);
        Console.WriteLine(sum);
    }

    long GetPossibilities(string s, int[] groups)
    {
        s = s.Trim('.');
        var key = s + string.Join('-', groups);
        if (cache.ContainsKey(key))
            return cache[key];

        var possibilities = 0L;
        if (groups.Length == 0 && s.All(c => c != '#'))
        {
            possibilities = 1;
            goto ret;
        }

        if (groups.Length == 0)
            goto ret;

        var gl = groups[0];
        var neededSpace = groups.Sum() + groups.Length - 1;

        var first = s.IndexOf("#");
        for (int i = 0; i < s.Length; i++)
        {
            if (s.Length - i < neededSpace)
                break;

            if (first >= 0 && i > first)
                goto ret;

            var matches = true;
            for (int j = i; j < i + gl; j++)
            {
                matches &= s[j] is '#' or '?';
            }

            if (matches && i + gl == s.Length)
            {
                possibilities++;
                goto ret;
            }
            else if (matches && s[i + gl] is '.' or '?')
            {
                var subPos = GetPossibilities(s[(i + gl + 1)..], groups[1..]);
                if (subPos > 0)
                {
                    possibilities += subPos;
                }
            }
        }

        ret:
        cache.TryAdd(key, possibilities);
        return possibilities;
    }

    private (string c, int[] control) Expand((string c, int[] control) arg)
    {
        var s = "";
        var l = new List<int>();

        for (int i = 0; i < 5; i++)
        {
            s += (i == 0 ? "" : "?") + arg.c;
            l.AddRange(arg.control);
        }

        return (c: s, control: l.ToArray());
    }

    class Check
    {
        public string Value { get; }
        public int[] Groups { get; }
        public long Possibilities { get; set; } = 0;

        public Check(string value, int[] groups)
        {
            Value = value;
            Groups = groups;
        }
    }
}
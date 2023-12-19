using System.Numerics;
using MM = (int min, int max);

public class Day19 : IDay
{
    private string[] lines;

    public Day19()
    {
        lines = File.ReadAllLines("day19/input.txt");
    }

    public void FirstPart()
    {
        var rules = lines.TakeWhile(l => !string.IsNullOrEmpty(l)).Select(l => new Rule(l)).ToDictionary(r => r.Name);
        var parts = lines.Skip(rules.Count + 1).Select(l => new Part(l)).ToList();

        foreach (var part in parts)
        {
            var r = "in";
            do
            {
                r = rules[r].Next(part);
            } while (r != "R" && r != "A");

            part.Accepted = r == "A";
        }

        Console.WriteLine(parts.Where(p => p.Accepted).Select(p => p.Variables.Values.Sum()).Sum());
    }

    public void SecondPart()
    {
        var rules = lines.TakeWhile(l => !string.IsNullOrEmpty(l)).Select(l => new Rule(l)).ToDictionary(r => r.Name);
        (MM x, MM m, MM a, MM s) range = ((1, 4000), (1, 4000), (1, 4000), (1, 4000));
        List<(MM x, MM m, MM a, MM s)> found = new List<(MM x, MM m, MM a, MM s)>();
        Stack<(Rule r, (MM x, MM m, MM a, MM s) s)> toCheck =
            new Stack<(Rule r, (MM x, MM m, MM a, MM s) s)>();

        toCheck.Push((rules["in"], range));
        do
        {
            (var r, range) = toCheck.Pop();

            foreach (var c in r.Conditions)
            {
                var m = c.MatchAndReduce(range);
                if (m.match)
                {
                    if (c.Target == "A")
                        found.Add(m.ranges[0]);
                    else if (c.Target != "R")
                        toCheck.Push((rules[c.Target], m.ranges[0]));

                    if (m.ranges.Length > 1)
                        range = m.ranges[1];
                }
            }
        } while (toCheck.Any());

        found.ForEach(r => Console.WriteLine(r));
        var combinations = found.Aggregate(new BigInteger(0),
            (a, r) => a + new BigInteger(r.x.max - r.x.min + 1) *
                new BigInteger(r.m.max - r.m.min + 1) *
                new BigInteger(r.a.max - r.a.min + 1) *
                new BigInteger(r.s.max - r.s.min + 1));
        Console.WriteLine(combinations);
    }

    class Part
    {
        public Dictionary<string, int> Variables = new();
        public bool Accepted { get; set; }

        public Part(string l)
        {
            Variables = l.Trim("{}".ToCharArray())
                .Split(',')
                .Select(v => v.Split('='))
                .ToDictionary(v => v[0], v => int.Parse(v[1]));
        }
    }

    class Rule
    {
        public Rule(string l)
        {
            var idx = l.IndexOf('{');
            Name = l[..idx];
            var cs = l[(idx + 1)..^1].Split(',');
            Conditions = cs.Select(c => new Condition(c)).ToList();
        }

        public List<Condition> Conditions { get; }

        public string Name { get; }

        public string Next(Part part)
        {
            foreach (var c in Conditions)
            {
                if (c.Matches(part))
                    return c.Target;
            }

            throw new Exception("xxx");
        }
    }

    class Condition
    {
        public int Value { get; }
        public Func<int, int, bool> Op { get; }
        public string? Ops { get; }
        public string? Variable { get; }
        public string Target { get; }

        public Condition(string c)
        {
            var idx = c.IndexOf(':');
            if (idx == -1)
            {
                Target = c;
                Op = (_, _) => true;
            }
            else
            {
                Target = c[(idx + 1)..];
                var op = c.IndexOfAny("><".ToCharArray());
                Ops = c[op].ToString();
                Variable = c[..op];
                Value = int.Parse(c[(op + 1)..idx]);
                Op = c[op] == '>' ? (x, y) => x > y : (x, y) => x < y;
            }
        }

        public bool Matches(Part part)
        {
            if (Variable == null)
                return Op(0, Value);

            return Op(part.Variables[Variable], Value);
        }

        public (bool match, (MM x, MM m, MM a, MM s)[] ranges) MatchAndReduce((MM x, MM m, MM a, MM s) c)
        {
            (MM x, MM m, MM a, MM s) ReplaceVar((MM x, MM m, MM a, MM s) xmas, MM minmax) =>
                Variable switch
                {
                    "x" => xmas with { x = minmax },
                    "m" => xmas with { m = minmax },
                    "a" => xmas with { a = minmax },
                    "s" => xmas with { s = minmax }
                };

            if (Ops is null)
                return (true, new[] { c });

            var v = Variable switch
            {
                "x" => c.x,
                "m" => c.m,
                "a" => c.a,
                "s" => c.s
            };

            var matchingV = v;
            var unmatchingV = v;
            if (Ops == "<" && v.min < Value)
            {
                matchingV = (v.min, Math.Min(v.max, Value - 1));
                unmatchingV = (Value, v.max);
                var matchingR = ReplaceVar(c, matchingV);
                var unmatchingR = ReplaceVar(c, unmatchingV);

                return (true, new[] { matchingR, unmatchingR });
            }

            if (Ops == ">" && v.max > Value)
            {
                matchingV = (Math.Max(v.min, Value + 1), v.max);
                unmatchingV = (v.min, Value);
                var matchingR = ReplaceVar(c, matchingV);
                var unmatchingR = ReplaceVar(c, unmatchingV);

                return (true, new[] { matchingR, unmatchingR });
            }

            return (false, new[] { c });
        }
    }
}
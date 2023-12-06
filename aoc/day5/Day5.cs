public class Day5 : IDay
{
    private string[] lines;


    public class Mapping
    {
        public string FromCategory { get; }
        public string ToCategory { get; }
        public (long start, long end) FromRange { get; }
        public (long start, long end) ToRange { get; }
        public long Range { get; }

        public Mapping(string fromCategory, string toCategory, long fromStart, long toStart, long range)
        {
            range = range > 0 ? range - 1 : 0;
            FromCategory = fromCategory;
            ToCategory = toCategory;
            FromRange = (fromStart, fromStart + range);
            ToRange = (toStart, toStart + range);
        }

        public long GetToLookupValue(long fromLookupValue)
        {
            return fromLookupValue - FromRange.start + ToRange.start;
        }

        public long GetFromLookupValue(long toLookupValue)
        {
            return toLookupValue - ToRange.start + FromRange.start;
        }
    }

    public Day5()
    {
        lines = File.ReadAllLines("day5/input.txt");
        
        Seeds = lines[0].Split(": ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Skip(1).Select(long.Parse).ToList();
        Mappings = new Dictionary<string, List<Mapping>>();

        var currentCategory = "seed";
        List<Mapping> currentMapping = null;
        string fromCategory = null, toCategory = null;
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.Contains("map"))
            {
                currentMapping = new List<Mapping>();
                var split = line.Split("- ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                fromCategory = split[0];
                toCategory = split[2];
                Mappings[fromCategory] = currentMapping;
                continue;
            }

            var numbers = line.Split().Select(long.Parse).ToArray();
            currentMapping.Add(new Mapping(fromCategory, toCategory, numbers[1], numbers[0], numbers[2]));
        }

        foreach (var mapping in Mappings)
        {
            backMapping[mapping.Value[0].ToCategory] = mapping.Key;
        }
    }

    public List<long> Seeds { get; set; }

    public Dictionary<string, List<Mapping>> Mappings { get; set; }

    public void FirstPart()
    {
        var locations = Seeds.Select(s => Follow(s, "seed", "location"));
        var min = locations.Min();
        Console.WriteLine(min);
    }

    private long Follow(long lookup, string currentCategory, string finalCategory)
    {
        //Console.Write(currentCategory + " " + lookup + " -- ");
        var mapping = Mappings[currentCategory].FirstOrDefault(m => lookup >= m.FromRange.start && lookup <= m.FromRange.end, GetDefaultMapping(lookup, currentCategory));
        
        
        lookup = mapping.GetToLookupValue(lookup);
        if (mapping.ToCategory == finalCategory)
        {
            //Console.WriteLine(finalCategory + " " + lookup);
            return lookup;
        }

        return Follow(lookup, mapping.ToCategory, finalCategory);
    }

    private Mapping GetDefaultMapping(long lookup, string currentCategory)
    {
        var to = Mappings[currentCategory][0].ToCategory;
        return new Mapping(currentCategory, to, lookup, lookup, 1);
    }

    private IDictionary<string, string> backMapping = new Dictionary<string, string>();

    private long FollowBack(long lookup, string currentCategory, string finalCategory)
    {
        var mapping = Mappings[backMapping[currentCategory]].FirstOrDefault(m => lookup >= m.ToRange.start && lookup <= m.ToRange.end, GetDefaultBackMapping(lookup, currentCategory));


        lookup = mapping.GetFromLookupValue(lookup);
        if (mapping.FromCategory == finalCategory)
        {
            return lookup;
        }

        return FollowBack(lookup, mapping.FromCategory, finalCategory);
    }

    private Mapping GetDefaultBackMapping(long lookup, string currentCategory)
    {
        return new Mapping(backMapping[currentCategory], currentCategory, lookup, lookup, 1);
    }

    public void SecondPart()
    {
        var ranges = new List<(int index, long start, long end)>();
        for (int i = 0; i < Seeds.Count; i += 2)
        {
            ranges.Add((i / 2, Seeds[i], Seeds[i] + Seeds[i + 1] - 1));
        }

        var minLocation = 0L;
        var maxLocation = Mappings["humidity"].Select(m => m.ToRange.end).Max();


        for (long location = minLocation; location < maxLocation; location++)
        {
            var seed = FollowBack(location, "location", "seed");
            if (ranges.Any(r => r.start <= seed && seed <= r.end))
            {
                Console.WriteLine("Found: " + seed + "@loc: " + location);
                break;
            }
        }
    }
}
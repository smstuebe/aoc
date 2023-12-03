public class Day3
{
    private string[] lines;

    class Number
    {
        public int Value { get; set; }
    }

    class Symbol
    {
        public string Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }


    class Manual
    {
        private object[][] content;
        public List<Symbol> Symbols { get; } = new List<Symbol>();

        public void Read(string[] lines)
        {
            var numbers = new List<Number>();
            content = new object[lines.Length][];

            for (int y = 0; y < lines.Length; y++)
            {
                content[y] = new object[lines[y].Length];
                Number currentNumber = new Number();
                for (int x = 0; x < lines[y].Length; x++)
                {
                    var value = lines[y][x];
                    if (value is >= '0' and <= '9')
                    {
                        currentNumber.Value = currentNumber.Value * 10 + value - '0';
                        content[y][x] = currentNumber;
                    }
                    else if (value == '.')
                    {
                        currentNumber = new Number();
                        continue;
                    }
                    else
                    {
                        currentNumber = new Number();
                        content[y][x] = new Symbol { Value = value.ToString(), X = x, Y = y };
                        Symbols.Add((Symbol)content[y][x]);
                    }
                }
            }
        }

        public List<Number> GetPartNumbers()
        {
            var numbers = new HashSet<Number>();
            foreach (var symbol in Symbols)
            {
                for (int y = symbol.Y - 1; y <= symbol.Y + 1; y++)
                {
                    for (int x = symbol.X - 1; x <= symbol.X + 1; x++)
                    {
                        if (y < 0 || y >= content.Length || x < 0 || x >= content[0].Length)
                            continue;

                        if (content[y][x] is Number n && !numbers.Contains(n))
                        {
                            numbers.Add(n);
                        }
                    }
                }
            }
            
            return numbers.ToList();
        }

        public List<int> GetGearRatios()
        {
            var ratios = new List<int>();
            foreach (var symbol in Symbols.Where(s => s.Value == "*"))
            {
                var numbers = new HashSet<Number>();
                for (int y = symbol.Y - 1; y <= symbol.Y + 1; y++)
                {
                    for (int x = symbol.X - 1; x <= symbol.X + 1; x++)
                    {
                        if (y < 0 || y >= content.Length || x < 0 || x >= content[0].Length)
                            continue;

                        if (content[y][x] is Number n && !numbers.Contains(n))
                        {
                            numbers.Add(n);
                        }
                    }
                }

                if (numbers.Count == 2)
                {
                    var n = numbers.Aggregate(1, (a, x) => a * x.Value);
                    ratios.Add(n);
                }
            }

            return ratios;
        }
    }


    public Day3()
    {
        lines = File.ReadAllLines("day3/input.txt");
    }

    public void FirstPart()
    {
        var manual = new Manual();
        manual.Read(lines);
        var sum = manual.GetPartNumbers().Sum(x => x.Value);
        Console.WriteLine(sum);
    }

    public void SecondPart()
    {
        var manual = new Manual();
        manual.Read(lines);
        var sum = manual.GetGearRatios()
            .Sum();

        Console.WriteLine(sum);
    }
}
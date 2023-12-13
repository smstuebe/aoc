public class Day13 : IDay
{
    private string[] lines;
    private List<Pattern> _patterns;

    public Day13()
    {
        lines = File.ReadAllLines("day13/input.txt");
        _patterns = new List<Pattern>();

        var list = new List<string>();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                _patterns.Add(new Pattern(list));
                list = new List<string>();
                continue;
            }

            list.Add(line);
        }

        _patterns.Add(new Pattern(list));
    }

    public void FirstPart()
    {
        var sum = _patterns.Select(p => p.GetReflectionValue(p.GetReflection)).Sum();
        Console.WriteLine(sum);
    }

    public void SecondPart()
    {
        var sum = _patterns.Select(p => p.GetReflectionValue(p.GetReflectionWithFix)).Sum();
        Console.WriteLine(sum);
    }

    class Pattern
    {
        public Pattern(List<string> rows)
        {
            Rows = rows;
            for (int i = 0; i < rows[0].Length; i++)
            {
                var col = "";
                foreach (var row in rows)
                {
                    col += row[i];
                }

                Cols.Add(col);
            }
        }

        public List<string> Rows { get; } = new();
        public List<string> Cols { get; } = new();

        public int GetReflectionValue(Func<List<string>, int> calcFn)
        {
            int row = calcFn(Rows);
            if (row > 0)
                return row * 100;

            return calcFn(Cols);
        }

        public int GetReflection(List<string> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                var width = Math.Min(i, list.Count - i);
                var r = true;
                for (int x = 0; x < width; x++)
                {
                    if (list[i - x - 1] != list[i + x])
                    {
                        r = false;
                        break;
                    }
                }

                if (r)
                    return i;
            }

            return 0;
        }

        public int GetReflectionWithFix(List<string> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                var width = Math.Min(i, list.Count - i);
                var d = 0;
                for (int x = 0; x < width; x++)
                {
                    var l = list[i - x - 1].Length;
                    for (int z = 0; z < l; z++)
                    {
                        if (list[i - x - 1][z] != list[i + x][z])
                            ++d;
                    }
                }

                if (d == 1)
                    return i;
            }
            
            return 0;
        }
    }
}
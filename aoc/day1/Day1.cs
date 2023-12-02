public class Day1
{
    private string[] lines;

    public Day1()
    {
        lines = File.ReadAllLines("day1/input.txt");
    }

    public void FirstPart()
    {
        var sum = lines.Select(l =>
                l.ToCharArray().Where(c => c is >= '0' and <= '9'))
            .Select(c => (c.First() - 0x30) * 10 + c.Last() - 0x30)
            .Sum();
        Console.WriteLine(sum);
    }

    public void SecondPart()
    {
        var digits = new[]{("0",0), ("1",1), ("2",2), ("3",3), ("4",4), ("5",5), ("6",6), ("7",7), ("8",8), ("9",9),
            ("one",1), ("two",2), ("three",3), ("four",4), ("five",5), ("six",6), ("seven",7), ("eight",8), ("nine",9)};


        var sum = lines
            .Select(l => digits
                .Select(d => (d, l.IndexOf(d.Item1), l.LastIndexOf(d.Item1)))
            )
            .Select(d => d.OrderBy(dpos => dpos.Item2).First(dpos => dpos.Item2 > -1).Item1.Item2 * 10 + d.OrderBy(dpos => dpos.Item3).Last(dpos => dpos.Item3 > -1).Item1.Item2)
            .Sum();

        Console.WriteLine(sum);
    }
}
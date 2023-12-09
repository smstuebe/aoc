public class Day9 : IDay
{
    private string[] lines;

    public Day9()
    {
        lines = File.ReadAllLines("day9/input.txt");
        Values = lines.Select(l => l.Split().Select(int.Parse).ToList()).ToList();
    }

    public List<List<int>> Values { get; set; }

    public void FirstPart()
    {

        var sequences = Values.Select(GetSequence).ToList();

        foreach (var sequence in sequences)
        {

            for (int j = sequence.Count - 2; j >= 0; j--)
            {
                sequence[j].Add(sequence[j + 1].Last() + sequence[j].Last());
            }
        }

        var sum = sequences.Select(s => s[0].Last()).Sum();

        Console.WriteLine(sum);
    }

    private List<List<int>> GetSequence(List<int> values)
    {
        var sequence = new List<List<int>>();
        sequence.Add(values);

    again:
        var allZero = true;
        var s = new List<int>();
        sequence.Add(s);
        for (int i = 0; i < values.Count - 1; i++)
        {
            var diff = values[i + 1] - values[i];
            s.Add(diff);
            allZero = diff == 0 && allZero;
        }

        if (!allZero)
        {
            values = sequence.Last();
            goto again;
        }

        return sequence;
    }

    public void SecondPart()
    {
        var sequences = Values.Select(GetSequence).ToList();

        foreach (var sequence in sequences)
        {

            for (int j = sequence.Count - 2; j >= 0; j--)
            {
                sequence[j].Insert(0,sequence[j].First() - sequence[j + 1].First());
            }
        }

        var sum = sequences.Select(s => s[0].First()).Sum();

        Console.WriteLine(sum);
    }
}
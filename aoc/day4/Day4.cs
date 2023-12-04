using System.Text.RegularExpressions;

public class Day4
{
    private string[] lines;


    class Card
    {
        public int Number { get; }
        private Regex cardRegex = new(@"^Card\s+\d+: ");

        public Card(string line)
        {
            line = cardRegex.Replace(line, "");
            var numbers = line.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            Winning = numbers[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
            My = numbers[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
            Matches = Winning.Intersect(My).Count();
            Points = Matches > 0 ? (int)Math.Pow(2, Matches - 1) : 0;
        }

        public int Points { get; set; }

        public int Matches { get; set; }

        public int[] My { get; set; }

        public int[] Winning { get; set; }
    }

    public Day4()
    {
        lines = File.ReadAllLines("day4/input.txt");
    }

    public void FirstPart()
    {
        var sum = lines.Select(l => new Card(l))
            .Sum(c => c.Points);
        Console.WriteLine(sum);
    }

    public void SecondPart()
    {
        var cards = lines.Select(l => new Card(l)).ToArray();
        var cardsWon = new int[cards.Length];

        for (int i = cards.Length-1; i >= 0; i--)
        {
            var matches = cards[i].Matches;
            if (matches == 0)
            {
                cardsWon[i] = 0;
                continue;
            }

            var won = Math.Min(matches, cards.Length - (i + 1));
            cardsWon[i] = won;

            for (int j = i+1; won > 0; j++, won--)
            {
                cardsWon[i] += cardsWon[j];
            }
        }

        var sum = cardsWon.Sum() + lines.Length;
        Console.WriteLine(sum);
    }
}
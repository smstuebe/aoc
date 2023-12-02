using System.Text.RegularExpressions;


public class Day2
{
    class Game
    {
        public int Id { get; set; }
        public (int, string)[] Cubes { get; set; }

        public static Game FromLine(string line)
        {
            var regex = new Regex(@"^Game (?<gameid>\d+): (?<cubes>.*)$");
            var game = new Game();


            var match = regex.Match(line);
            game.Id = int.Parse(match.Groups["gameid"].Value);

            var cubes = match.Groups["cubes"].Value.Split(';', ',');
            game.Cubes = cubes.Select(c =>
            {
                var a = c.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                return (int.Parse(a[0]), a[1]);
            }).ToArray();

            return game;
        }

        public int Power()
        {
            var max = Cubes.GroupBy(g => g.Item2).Select(g => g.MaxBy(g1 => g1.Item1)).ToArray();
            if (max.Length < 3)
                throw new AggregateException("dfsdf");

            return max[0].Item1 * max[1].Item1 * max[2].Item1;
        }
    }

    private string[] lines;

    public Day2()
    {
        lines = File.ReadAllLines("day2/input.txt");
    }

    public void FirstPart()
    {
        int maxBlue = 14, maxGreen = 13, maxRed = 12;
        var games = lines
            .Select(Game.FromLine)
            .Where(g =>
                !g.Cubes.Any(c => c.Item2 == "green" && c.Item1 > maxGreen) &&
                !g.Cubes.Any(c => c.Item2 == "red" && c.Item1 > maxRed) &&
                !g.Cubes.Any(c => c.Item2 == "blue" && c.Item1 > maxBlue));

        var sum = games.Sum(g => g.Id);

        Console.WriteLine(sum);
    }

    public void SecondPart()
    {
        var sum = lines
            .Select(Game.FromLine)
            .Select(g => g.Power())
            .Sum();

        Console.WriteLine(sum);
    }
}
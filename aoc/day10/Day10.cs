public class Day10 : IDay
{
    private string[] lines;
    private List<List<Pipe>> _pipes;
    private List<Pipe> _flatPipes;
    private Pipe _startPipe;
    private readonly List<Pipe> _loop;


    public Day10()
    {
        lines = File.ReadAllLines("day10/input.txt");

        _pipes = new List<List<Pipe>>();

        var start = (x: 0, y: 0);
        for (int y = 0; y < lines.Length; y++)
        {
            _pipes.Add(new List<Pipe>());
            for (int x = 0; x < lines[y].Length; x++)
            {
                _pipes[y].Add(new Pipe(lines[y][x], new Position(x, y)));
                if (lines[y][x] == 'S')
                {
                    start = (x, y);
                }
            }
        }

        _flatPipes = _pipes.SelectMany(p => p).ToList();
        _startPipe = _pipes[start.y][start.x];
        _startPipe.ConnectsTo = _flatPipes.Where(p => p.ConnectsTo.Any(pos => pos == _startPipe.Position)).Select(p => p.Position).ToArray();

        _loop = new List<Pipe>();
        _loop.Add(_startPipe);
        var nextPosition = _startPipe.ConnectsTo[0];
        var lastPosition = _startPipe.Position;
        do
        {
            _loop.Add(_pipes[nextPosition.y][nextPosition.x]);
            var current = nextPosition;
            nextPosition = _pipes[nextPosition.y][nextPosition.x].NextPosition(lastPosition);
            lastPosition = current;
        } while (nextPosition != _startPipe.Position);
    }

    public void FirstPart()
    {
        Console.WriteLine(_loop.Count / 2);
    }

    public void SecondPart()
    {
        var ground = _flatPipes.Except(_loop);
        var enclosed = ground.Where(IsEnclosed).ToList();
        foreach (var pipe in enclosed)
        {
            Console.WriteLine(pipe.Position);
        }
        Console.WriteLine(enclosed.Count);
    }

    private bool IsEnclosed(Pipe p)
    {
        var collisions = 0;
        var y = p.Position.y;
        for (int x = p.Position.x + 1; x < _pipes[y].Count; x++)
        {
            var currentPipe = _pipes[y][x];
            if (!_loop.Contains(currentPipe))
                continue;

            if (currentPipe.Type == '|')
                collisions++;


            if (currentPipe.Type is not ('L' or 'F'))
                continue;

            var startType = currentPipe.Type;
            do
            {
                currentPipe = _pipes[y][++x];
            } while (currentPipe.Type == '-');

            if ((startType == 'L' && currentPipe.Type == '7') ||
                (startType == 'F' && currentPipe.Type == 'J'))
                collisions++;
        }

        return collisions % 2 != 0;
    }


    class Pipe
    {
        public char Type { get; set; }
        public Position Position { get; set; }
        public Position[] ConnectsTo { get; set; } = new Position[2];

        public Pipe(char type, Position position)
        {
            Type = type;
            Position = position;

            var x = position.x;
            var y = position.y;
            ConnectsTo = type switch
            {
                '|' => new Position[] { new(x, y - 1), new(x, y + 1) }, // is a vertical pipe connecting north and south.
                '-' => new Position[] { new(x - 1, y), new(x + 1, y) }, // is a horizontal pipe connecting east and west.
                'L' => new Position[] { new(x, y - 1), new(x + 1, y) }, // is a 90 - degree bend connecting north and east.
                'J' => new Position[] { new(x, y - 1), new(x - 1, y) }, // is a 90 - degree bend connecting north and west.
                '7' => new Position[] { new(x, y + 1), new(x - 1, y) }, // is a 90 - degree bend connecting south and west.
                'F' => new Position[] { new(x, y + 1), new(x + 1, y) }, // is a 90 - degree bend connecting south and east.
                '.' => new Position[] { }, // is ground; there is no pipe in this tile.
                'S' => new Position[] { }, // is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
            };
        }

        public Position NextPosition(Position currentPosition)
        {
            return currentPosition == ConnectsTo[0] ? ConnectsTo[1] : ConnectsTo[0];
        }
    }

    record Position(int x, int y);
}
public class Day14 : IDay
{
    private char[][] lines;

    public Day14()
    {
        lines = File.ReadAllLines("day14/input.txt")
            .Select(s => s.ToCharArray())
            .ToArray();
    }

    public void FirstPart()
    {
        var totalWeight = 0;
        for (int c = 0; c < lines[0].Length; c++)
        {
            var weight = lines.Length;
            for (var r = 0; r < lines.Length; r++)
            {
                if (lines[r][c] == 'O')
                {
                    totalWeight += weight;
                    weight--;
                }

                if (lines[r][c] == '#')
                {
                    weight = lines.Length - r - 1;
                }
            }
        }

        Console.WriteLine(totalWeight);
    }

    public void SecondPart()
    {
        // Number is correct, because I was lucky.
        var flat = lines.SelectMany(x => x.Select(c => (byte)c)).ToArray();
        for (int i = 0; i < 1000000000; i++)
        {
            (flat, var hitN) = MoveNorth(flat);
            (flat, var hitW) = MoveWest(flat);
            (flat, var hitS) = MoveSouth(flat);
            (flat, var hitE) = MoveEast(flat);
            if (i % 1000000 == 0)
            {
                Console.WriteLine($"{i / 10000000f}%, n:{northCache.Count}, s:{southCache.Count}, w:{westCache.Count}, e:{eastCache.Count}");
                if (hitN && hitE && hitS && hitW && 1000000000 % i == 0)
                {
                    Console.WriteLine(Weight(flat) + " --> " + i);
                }
            }
        }
        
        Console.WriteLine(Weight(flat));
    }

    private int Weight(byte[] flat)
    {
        var weight = 0;
        var width = lines[0].Length;
        var height = lines.Length;
        for (int c = 0; c < width; c++)
        {
            for (var r = 0; r < height; r++)
            {
                if (flat[r * width + c] == 'O')
                {
                    weight += height - r;
                }
            }
        }

        return weight;
    }

    private Dictionary<byte[], byte[]> northCache = new(new ByteArrayComparer());
    private Dictionary<byte[], byte[]> southCache = new(new ByteArrayComparer());
    private Dictionary<byte[], byte[]> westCache = new(new ByteArrayComparer());
    private Dictionary<byte[], byte[]> eastCache = new(new ByteArrayComparer());

    private (byte[] bytes, bool hit) MoveNorth(byte[] bytes)
    {
        if (northCache.TryGetValue(bytes, out var x))
            return (x, true);

        var newBytes = new byte[bytes.Length];
        var width = lines[0].Length;
        var height = lines.Length;
        for (int c = 0; c < width; c++)
        {
            var putHere = 0;
            for (var r = 0; r < height; r++)
            {
                if (bytes[r * width + c] == 'O')
                {
                    newBytes[putHere * width + c] = (byte)'O';
                    putHere++;
                }
                else if (bytes[r * width + c] == '#')
                {
                    newBytes[r * width + c] = (byte)'#';
                    putHere = r + 1;
                }
            }
        }

        northCache.Add(bytes, newBytes);
        return (newBytes, false);
    }

    private (byte[] bytes, bool hit) MoveSouth(byte[] bytes)
    {
        if (southCache.TryGetValue(bytes, out var x))
            return (x, true);

        var newBytes = new byte[bytes.Length];
        var width = lines[0].Length;
        var height = lines.Length;
        for (int c = 0; c < width; c++)
        {
            var putHere = height-1;
            for (var r = height - 1; r >= 0; r--)
            {
                if (bytes[r * width + c] == 'O')
                {
                    newBytes[putHere * width + c] = (byte)'O';
                    putHere--;
                }
                else if (bytes[r * width + c] == '#')
                {
                    newBytes[r * width + c] = (byte)'#';
                    putHere = r - 1;
                }
            }
        }

        southCache.Add(bytes, newBytes);
        return (newBytes, false);
    }

    private (byte[] bytes, bool hit) MoveWest(byte[] bytes)
    {
        if (westCache.TryGetValue(bytes, out var x))
            return (x, true);

        var newBytes = new byte[bytes.Length];
        var width = lines[0].Length;
        var height = lines.Length;
        for (var r = 0; r < height; r++)
        {
            var putHere = 0;
            for (int c = 0; c < width; c++)
            {
                if (bytes[r * width + c] == 'O')
                {
                    newBytes[r * width + putHere] = (byte)'O';
                    putHere++;
                }
                else if (bytes[r * width + c] == '#')
                {
                    newBytes[r * width + c] = (byte)'#';
                    putHere = c + 1;
                }
            }
        }

        westCache.Add(bytes, newBytes);
        return (newBytes, false);
    }

    private (byte[] bytes, bool hit) MoveEast(byte[] bytes)
    {
        if (eastCache.TryGetValue(bytes, out var x))
            return (x, true);

        var newBytes = new byte[bytes.Length];
        var width = lines[0].Length;
        var height = lines.Length;
        for (var r = 0; r < height; r++)
        {
            var putHere = width -1;
            for (int c = width-1; c >=0 ; c--)
            {
                if (bytes[r * width + c] == 'O')
                {
                    newBytes[r * width + putHere] = (byte)'O';
                    putHere--;
                }
                else if (bytes[r * width + c] == '#')
                {
                    newBytes[r * width + c] = (byte)'#';
                    putHere = c - 1;
                }
            }
        }

        eastCache.Add(bytes, newBytes);
        return (newBytes, false);
    }
    
    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] x, byte[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(byte[] obj)
        {
            var h = new HashCode();
            h.AddBytes(obj);
            return h.ToHashCode();
        }
    }
}
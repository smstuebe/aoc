public class Day7 : IDay
{
    private string[] lines;

    public Day7()
    {
        lines = File.ReadAllLines("day7/input.txt");
        Games = lines.Select(l =>
        {
            var x = l.Split();
            return (hand: new Hand(x[0]), bid: int.Parse(x[1]));
        }).ToList();
    }

    public List<(Hand hand, int bid)> Games { get; }

    public void FirstPart()
    {
        var sum = Games.OrderBy(x => x.hand, new HandComparer(h => h.Type, (hand, i) => hand.CardValueAt(i)))
            .Select((g, i) => (i+1) * g.bid)
            .Sum();

        Console.WriteLine(sum);
    }

    public void SecondPart()
    {
        var sum = Games.OrderBy(x => x.hand, new HandComparer(h => h.BoostedHandType, (hand, i) => hand.CardJIsALooserValueAt(i)))
            .Select((g, i) => (i+1) * g.bid)
            .Sum();

        Console.WriteLine(sum);
    }
    
    public enum HandType
    {
        HighCard,
        Pair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }
    
    public class Hand
    {
        public string Value { get; }
        public HandType Type { get; }
        
        public HandType BoostedHandType { get; }

        public Hand(string value)
        {
            Value = value;
            Type = GetHandType();
            BoostedHandType = GetBoostedHandType();
        }

        private HandType GetHandType()
        {
            var groups = Value.GroupBy(v => v).ToList();
            if (groups.Count == 1)
                return HandType.FiveOfAKind;

            if (groups.Count == 2)
            {
                if (groups.Any(g => g.Count() == 4))
                    return HandType.FourOfAKind;

                return HandType.FullHouse;
            }

            if (groups.Any(g => g.Count() == 3))
                return HandType.ThreeOfAKind;

            var pairs = groups.Count(g => g.Count() == 2);
            return pairs switch
            {
                2 => HandType.TwoPair,
                1 => HandType.Pair,
                _ => HandType.HighCard
            };
        }
        
        private HandType GetBoostedHandType()
        {
            var jokers = Value.Count(x => x == 'J');
            if (jokers == 0 || Type == HandType.FiveOfAKind)
            {
                return Type;
            }

            if (Type is HandType.FourOfAKind or HandType.FullHouse)
            {
                return HandType.FiveOfAKind;
            }

            if (Type == HandType.ThreeOfAKind)
            {
                return HandType.FourOfAKind;
            }
            
            if (Type == HandType.TwoPair)
            {
                return jokers == 2 ? HandType.FourOfAKind : HandType.FullHouse;
            }

            return Type == HandType.Pair ? HandType.ThreeOfAKind : HandType.Pair;
        }

        public int CardValueAt(int index)
        {
            return Value[index] switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => 11,
                'T' => 10,
                _ => Value[index] - '0'
            };
        }
        
        public int CardJIsALooserValueAt(int index)
        {
            return Value[index] switch
            {
                'J' => 0,
                _ => CardValueAt(index)
            };
        }
    }

    public class HandComparer : IComparer<Hand>
    {
        private readonly Func<Hand, HandType> _handTypeSelector;
        private readonly Func<Hand, int, int> _cardValueSelector;

        public HandComparer(Func<Hand, HandType> handTypeSelector, Func<Hand, int, int> cardValueSelector)
        {
            _handTypeSelector = handTypeSelector;
            _cardValueSelector = cardValueSelector;
        }
        
        public int Compare(Hand x, Hand y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            
            if (_handTypeSelector(x) != _handTypeSelector(y))
            {
                return (int)_handTypeSelector(x) - (int)_handTypeSelector(y);
            }

            for (int i = 0; i < 5; i++)
            {
                var xv = _cardValueSelector(x, i);
                var yv = _cardValueSelector(y, i);
                var diff = xv - yv;
                if (diff != 0)
                    return diff;
            }

            return 0;
        }
    }
}

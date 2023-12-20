using System.Text.RegularExpressions;

public class Day20 : IDay
{
    private string[] lines;

    public Day20()
    {
        lines = File.ReadAllLines("day20/input.txt");
    }

    public void FirstPart()
    {
        Dictionary<string, Module> modules = new();
        var regex = new Regex(@"(\w+)");
        foreach (var line in lines)
        {
            var m = regex.Matches(line).Select(x => x.Value).ToList();
            var t = line[0];

            foreach (var mod in m)
            {
                if(!modules.ContainsKey(mod)) 
                    modules.Add(mod, new Module());
            }

            var fm = modules[m.First()];
            fm.Out.AddRange(m.Skip(1));
            fm.Type = t;
        }

        foreach (var mod in modules)
        {
            mod.Value.SetInputs(modules.Where(m => m.Value.Out.Contains(mod.Key)).Select(m => m.Key));
            mod.Value.Name = mod.Key;
        }

        Queue<(string from, int pulse, string target)> signals = new();
        var cnt = new[] { 0L, 0L, 0L };

        void Send(string from, int pulse, string target)
        {
            cnt[pulse + 1]++;
            //Console.WriteLine($"{from} {pulse} -> {target} -- low: {cnt[0]}, high: {cnt[2]}");
            signals.Enqueue((from, pulse, target));
        }
        
        for (int i = 0; i < 1000; i++)
        {
            Send("button", -1, "broadcaster");
            while (signals.Any())
            {
                var s = signals.Dequeue();
                modules[s.target].Process(s.from, s.pulse, Send);
            }
        }
        
        Console.WriteLine($"low: {cnt[0]}, high: {cnt[2]}, result = {cnt[0]*cnt[2]}");
    }

    public void SecondPart()
    {
    }

    class Module
    {
        public List<string> Out { get; set; } = new();
        public Dictionary<string, int> In { get; set; } = new();
        
        public char Type { get; set; }
        public string Name { get; set; }

        private bool on;
        
        public void Process(string from, int pulse, Action<string, int, string> send)
        {
            void SendAll(int p) => Out.ForEach(o => send(Name, p, o));
            if (Type == '%')
            {
                if (pulse == -1)
                {
                    on = !on;
                    SendAll(on ? 1 : -1);
                }
            }
            else if(Type == '&')
            {
                In[from] = pulse;
                var p = In.Values.All(v => v == 1) ? -1 : 1;
                SendAll(p);
            }
            else
                SendAll(pulse);
        }

        public void SetInputs(IEnumerable<string> ins) => 
            In = ins.ToDictionary(k => k, _ => -1);
    }
}
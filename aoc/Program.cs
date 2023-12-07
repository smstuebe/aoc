using System.Reflection;

var currentDay = Assembly.GetExecutingAssembly()
    .DefinedTypes
    .Where(t => t.IsClass && t.Name.StartsWith("Day") && !t.Name.EndsWith("X"))
    .Select<TypeInfo, (TypeInfo, int)>(t => (type: t, index: int.Parse(t.Name[3..])))
    .MaxBy(x => x.Item2);

Console.WriteLine($"Starting day {currentDay.Item2}...");
var day = (IDay)Activator.CreateInstance(currentDay.Item1)!;
day.FirstPart();
day.SecondPart();

Console.ReadKey();
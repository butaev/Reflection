using System.Diagnostics;

namespace Reflection;

public static class Program
{
    private static void Main()
    {
        const int iterations = 100000;

        var f1 = F.Get();

        var csvSerializeTime = MeasureTime(() => CsvSerializer.Serialize(f1), iterations);
        Console.WriteLine("Csv serialize time: {0} мс", csvSerializeTime.totalTime);

        var jsonSerializeTime = MeasureTime(() => System.Text.Json.JsonSerializer.Serialize(f1), iterations);
        Console.WriteLine("System.Text.Json serialize time: {0} мс", jsonSerializeTime.totalTime);
        
        var csvF = File.ReadAllText("f.csv");
        var csvDeserializeTime = MeasureTime(() => CsvSerializer.Deserialize<F>(csvF), iterations);
        Console.WriteLine("Csv deserialize time: {0} мс", csvDeserializeTime.totalTime);

        var jsonF = File.ReadAllText("f.json");
        var jsonDeserializeTime = MeasureTime(() => System.Text.Json.JsonSerializer.Deserialize<F>(jsonF), iterations);
        Console.WriteLine("System.Text.Json deserialize time: {0} мс", jsonDeserializeTime.totalTime);
    }

    private static (long totalTime, double averageTime) MeasureTime(Action action, long iterations)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            action();
        }

        stopwatch.Stop();
        return (stopwatch.ElapsedMilliseconds, (double)stopwatch.ElapsedMilliseconds / iterations);
    }
}
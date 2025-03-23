using ParameterListComparer;
using System.Collections.Concurrent;

var topMatchResultsRequired = 2;

var inputFolderPath = @"../../../Inputs";
var inputDevices = JsonReader.ReadAllFiles(inputFolderPath);

var libraryFolderPath = @"../../../Library";
var libraryDevices = JsonReader.ReadAllFiles(libraryFolderPath);

foreach (var inputDevice in inputDevices)
{
    var maximumMatchCount = inputDevice.DeviceParameterPropertyCount * inputDevice.Parameters.Count;
    var topMatches = FindBestMatches(inputDevice, libraryDevices, topMatchResultsRequired);
    PrintResults(inputDevice.Name, maximumMatchCount, topMatches);
}

List<(string Name, int Score, List<string> Differences)> FindBestMatches(Device inputDevice, List<Device> libraryDevices, int topN)
{
    var matches = new ConcurrentBag<(string Name, int Score, List<string> Differences)>();

    Parallel.ForEach(libraryDevices, libraryDevice =>
    {
        var currentMatchCount = 0;
        var currentDeviceDifferences = new List<string>();

        foreach (var inputParameter in inputDevice.Parameters)
        {
            if (libraryDevice.Parameters.TryGetValue(inputParameter.Key, out var libraryParameter))
            {
                (var score, var differences) = inputParameter.Value.Compare(libraryParameter);
                currentMatchCount += score;
                currentDeviceDifferences.AddRange(differences);
            }
            else
            {
                currentDeviceDifferences.Add($"Address \"{inputParameter.Key}\" missing in library");
            }
        }

        matches.Add((libraryDevice.Name, currentMatchCount, currentDeviceDifferences));
    });

    return matches.OrderByDescending(m => m.Score).Take(topN).ToList();
}

void PrintResults(string inputDeviceName, int maximumMatchCount, List<(string Name, int Score, List<string> Differences)> topMatches)
{
    Console.WriteLine($"Input file name: {inputDeviceName}");
    foreach (var match in topMatches)
    {
        Console.WriteLine($"- Matched with: {match.Name}, score: {((double)match.Score / maximumMatchCount * 100):F2}%\n");
        if (match.Differences.Count > 0)
        {
            Console.WriteLine("- Differences:");
            match.Differences.ForEach(item => Console.WriteLine("\t" + item));
            Console.WriteLine();
        }
    }

    Console.WriteLine("----------------------------------------\n");
}
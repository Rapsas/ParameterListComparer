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

List<(string Name, CompareResults CompareResults)> FindBestMatches(Device inputDevice, List<Device> libraryDevices, int topN)
{
    var matches = new ConcurrentBag<(string Name, CompareResults CompareResults)>();
    Parallel.ForEach(libraryDevices, libraryDevice => matches.Add(inputDevice.Compare(libraryDevice)));

    return matches.OrderByDescending(m => m.CompareResults.Score).Take(topN).ToList();
}

void PrintResults(string inputDeviceName, int maximumMatchCount, List<(string Name, CompareResults CompareResults)> topMatches)
{
    Console.WriteLine($"Input file name: {inputDeviceName}");
    foreach (var match in topMatches)
    {
        Console.WriteLine($"- Matched with: {match.Name}, score: {((double)match.CompareResults.Score / maximumMatchCount * 100):F2}%\n");
        if (match.CompareResults.Differences.Count > 0)
        {
            Console.WriteLine("- Differences:");
            match.CompareResults.Differences.ForEach(item => Console.WriteLine("\t" + item));
            Console.WriteLine();
        }
    }

    Console.WriteLine("-------------------------------------------\n");
}
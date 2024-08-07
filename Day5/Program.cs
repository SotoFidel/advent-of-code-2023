using System.Diagnostics;
using System.Globalization;

public class Mapping
{
    public Mapping(long destination, long source, long range)
    {
        Destination = destination;
        Source = source;
        Range = range;
    }
    public long Destination { get; set; }
    public long Source { get; set; }
    public long Range { get; set; }
}

class Program
{

    private static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: [Part1|Part2] [FileName]");
            return;
        }

        switch (args[0])
        {
            case "Part1":
                Part1(args[1]);
                break;
            case "Part2":
                Part2(args[1]);
                break;
            default:
                Console.WriteLine("Usage: [Part1|Part2] [FileName]");
                return;
        }
    }

    static void Part1(string fileName)
    {

        Tuple<StreamReader?, Exception?> ioResult = OpenFile(fileName);
        if (ioResult.Item1 == null)
        {
            Console.WriteLine(GetFullError(ioResult.Item2));
            return;
        }
        
        StreamReader streamReader = ioResult.Item1;
        List<string> lines = streamReader.ReadToEnd().Split('\n').ToList();
        streamReader.Close();

        List<long> seedsToPlant = [];
        List<string> seedsInput = lines[0].Split(' ').ToList();
        seedsInput.RemoveAt(0);
        foreach(string numString in seedsInput)
        {
            seedsToPlant.Add(long.Parse(numString));
        }

        Dictionary<string,List<Mapping>> mappings = ConstructDictionary(lines);

        for(int i = 0; i < seedsToPlant.Count; i++)
        {
            foreach(KeyValuePair<string,List<Mapping>> seedMappings in mappings)
            {
                foreach(Mapping step in seedMappings.Value)
                {
                    if(isBetween(seedsToPlant[i],step.Source,step.Source + step.Range -1))
                    {
                        seedsToPlant[i] = step.Destination + Math.Abs(seedsToPlant[i] - step.Source);
                        break;
                    }
                }
            }
        }

        long min = seedsToPlant[0];
        for(int i = 0; i < seedsToPlant.Count; i++)
        {
            if(min >= seedsToPlant[i])
            {
                min = seedsToPlant[i];
            }
        }

        Console.WriteLine("Min: " + min);
        return;

    }
    
    static void Part2(string fileName)
    {
        Tuple<StreamReader?,Exception?> ioResult = OpenFile(fileName);
        if (ioResult.Item2 != null)
        {
            Console.WriteLine(GetFullError(ioResult.Item2));
            return;
        }

        List<string> lines = ioResult.Item1!.ReadToEnd().Split('\n').ToList();
        ioResult.Item1.Close();

        List<(long seedValue, long seedRange)> seedRanges = new();
        List<string> seedLine = lines[0].Split(' ').ToList();
        seedLine.RemoveAt(0);

        for(int i = 0; i < seedLine.Count - 1; i+=2)
        {
            seedRanges.Add((long.Parse(seedLine[i]),long.Parse(seedLine[i+1])));
        }

        Dictionary<string,List<Mapping>> mappings = ConstructDictionary(lines).Reverse().ToDictionary();
        List<Mapping> locations = mappings["humidity-to-location"];
        mappings.Remove("humidity-to-location");

        long min = 0;
        bool seedFound = false;
        long currentMapping;
        for(long i = 0;; i++)
        {
            currentMapping = i;
            foreach(KeyValuePair<string,List<Mapping>> seedMappings in mappings)
            {
                foreach(Mapping step in seedMappings.Value)
                {
                    if(isBetween(currentMapping,step.Destination,step.Destination + step.Range - 1))
                    {
                        currentMapping = step.Source + Math.Abs(currentMapping - step.Destination);
                        break;
                    }
                }
            }

            for(int j = 0; j < seedRanges.Count; j++)
            {
                if(isBetween(currentMapping,seedRanges[j].seedValue,seedRanges[j].seedValue + seedRanges[j].seedRange - 1))
                {
                    seedFound = true;
                    break;
                }
            }

            if (seedFound) {min = i; break;}
        }

        Console.WriteLine("Min: " + min);

        return;
    }

    static Tuple<StreamReader?, Exception?> OpenFile(string fileName)
    {
        try
        {
            StreamReader streamReader = new StreamReader(fileName);
            return new Tuple<StreamReader?, Exception?>(streamReader, null);
        }
        catch (Exception e)
        {
            return new Tuple<StreamReader?, Exception?>(null, e);
        }
    }

    static Dictionary<string,List<Mapping>> ConstructDictionary(List<string> lines)
    {

        Dictionary<string,List<Mapping>> mappings = new()
        {
            {"seed-to-soil",new()},
            {"soil-to-fertilizer",new()},
            {"fertilizer-to-water",new()},
            {"water-to-light",new()},
            {"light-to-temperature",new()},
            {"temperature-to-humidity",new()},
            {"humidity-to-location",new()}
        };
        string mappingName;
        string[] parsedMappingEntry;

        for(int i = 0; i < lines.Count; i++)
        {
            mappingName = lines[i].Split(' ')[0];
            if(mappings.ContainsKey(mappingName))
            {
                i++;
                while(i < lines.Count && !string.IsNullOrWhiteSpace(lines[i]))
                {
                    parsedMappingEntry = lines[i].Split(' ');
                    mappings[mappingName].Add(new(long.Parse(parsedMappingEntry[0]),
                                                    long.Parse(parsedMappingEntry[1]),
                                                    long.Parse(parsedMappingEntry[2])));
                    i++;
                }
            }
        }

        return mappings;
    }

    static string GetFullError(Exception? ex)
    {
        if (ex == null) { return ""; }
        return ex.InnerException == null
                ? ex.Message
                : ex.Message + " --> " + GetFullError(ex.InnerException);
    }

    static bool isBetween(long num, long min, long max)
    {
        return num >= min && num <= max;
    }

}
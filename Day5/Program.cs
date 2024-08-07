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

        Dictionary<string,List<Mapping>> mappings = ConstructDictionary(lines);

        long? min = null;
        string phase;
        List<int> processedIndeces = new();

        foreach(KeyValuePair<string,List<Mapping>> seedMappings in mappings)
        {

            processedIndeces.Clear();
            phase = seedMappings.Key;
            foreach(Mapping step in seedMappings.Value)
            {

                for(int i = 0; i < seedRanges.Count; i++)
                {
                    if(processedIndeces.Contains(i))
                    {
                        continue;
                    }

                    // This seed range is inside the range step
                    if(seedRanges[i].seedValue >= step.Source 
                        && seedRanges[i].seedValue + seedRanges[i].seedRange - 1 <= step.Source + step.Range - 1)
                    {
                        processedIndeces.Add(i);
                        (long seedValue,long seedRange) temp = seedRanges[i];
                        temp.seedValue = step.Destination + Math.Abs(temp.seedValue - step.Source);
                        seedRanges[i] = temp;
                    }
                    else if(seedRanges[i].seedValue >= step.Source
                            && seedRanges[i].seedValue <= step.Source + step.Range - 1)
                    {
                        processedIndeces.Add(i);
                        (long seedValue,long seedRange) rightRange = (step.Source + step.Range, (seedRanges[i].seedValue + seedRanges[i].seedRange) - (step.Source + step.Range));
                        (long seedValue,long seedRange) leftRange = (step.Destination + seedRanges[i].seedValue - step.Source,rightRange.seedValue - seedRanges[i].seedValue);
                        seedRanges[i] = leftRange;
                        seedRanges.Add(rightRange);
                    }
                    else if(seedRanges[i].seedValue + seedRanges[i].seedRange - 1 <= step.Source + step.Range - 1
                            && seedRanges[i].seedValue + seedRanges[i].seedRange - 1 >= step.Source)
                    {
                        processedIndeces.Add(i);
                        (long seedValue,long seedRange) leftRange = (seedRanges[i].seedValue,step.Source - seedRanges[i].seedValue);
                        (long seedValue,long seedRange) rightRange = (step.Destination,Math.Abs( (seedRanges[i].seedValue + seedRanges[i].seedRange) - step.Source));
                        seedRanges[i] = rightRange;
                        seedRanges.Add(leftRange);
                    }
                    else if(seedRanges[i].seedValue < step.Source && 
                            seedRanges[i].seedValue + seedRanges[i].seedRange - 1 > step.Source + step.Range - 1) 
                    {
                        processedIndeces.Add(i);
                        (long seedValue,long seedRange) leftRange;
                        (long seedValue,long seedRange) middleRange;
                        (long seedValue,long seedRange) rightRange;

                        leftRange = (seedRanges[i].seedValue,step.Source - seedRanges[i].seedValue);
                        rightRange = (step.Source + step.Range,(seedRanges[i].seedValue + seedRanges[i].seedRange)-(step.Source+step.Range));
                        middleRange = (step.Destination,step.Range);

                        seedRanges[i] = middleRange;
                        seedRanges.Add(leftRange);
                        seedRanges.Add(rightRange);
                    }
                }
            }
        }

        min = seedRanges[0].seedValue;
        for(int i = 1; i < seedRanges.Count; i++)
        {
            if(min > seedRanges[i].seedValue)
            {
                min = seedRanges[i].seedValue;
            }
        }

        Console.WriteLine("min: " + min);

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
class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: [Part1|Part2] [FileName]");
            return;
        }

        (StreamReader? streamReader, Exception? exception) = OpenFile(args[1]);
        if(exception != null)
        {
            Console.WriteLine(GetFullError(exception));
            return;
        }

        string input = streamReader!.ReadToEnd();
        streamReader.Close();
        List<List<int>> readings = input.Split("\r\n")
                                    .Select(l => l.Split(' ')
                                                    .Select(e => int.Parse(e))
                                                    .ToList()
                                    ).ToList();
        
        int extrapolatedValuesSum = 0;
        switch (args[0])
        {
            case "Part1":
                extrapolatedValuesSum = Part1(readings);
                break;
            case "Part2":
                extrapolatedValuesSum = Part2(readings);
                break;
            default:
                Console.WriteLine("Usage: [Part1|Part2] [FileName]");
                return;
        }

        Console.WriteLine("Sum of extrapolated values: " + extrapolatedValuesSum);

    }

    static int Part1(List<List<int>> readings)
    {
        int extrapolatedValuesSum = 0;

        foreach(List<int> readingSet in readings)
        {
            extrapolatedValuesSum += readingSet.Last() + Part1Process(readingSet);
        }

        return extrapolatedValuesSum;
    }

    static int Part2(List<List<int>> readings)
    {
        int extrapolatedValuesSum = 0;
        foreach(List<int> readingSet in readings)
        {
            readingSet.Insert(0,readingSet.First() - Part2Process(readingSet));
            extrapolatedValuesSum += readingSet[0];
        }

        return extrapolatedValuesSum;
    }


    static int Part1Process(List<int> readings)
    {
        List<int> differences = [];
        for(int i = 0; i < readings.Count - 1; i++)
        {
            differences.Add(readings[i+1] - readings[i]);
        }

        if(!differences.All(d => d == 0))
        {
            differences.Add(differences.Last() + Part1Process(differences));
        }

        return differences.Last();
    }

    static int Part2Process(List<int> readings)
    {
        List<int> differences = [];
        for(int i = 0; i < readings.Count - 1; i++)
        {
            differences.Add(readings[i+1] - readings[i]);
        }

        if(!differences.All(d => d == 0))
        {
            differences.Insert(0,differences.First() - Part2Process(differences));
        }

        return differences.First();
    }


#region Utils
    static Tuple<StreamReader?,Exception?> OpenFile(string fileName)
    {
        try
        {
            StreamReader streamReader = new StreamReader(fileName);
            return new (streamReader, null);
        }
        catch (Exception e)
        {
            return new (null, e);
        }
    }

    static string GetFullError(Exception? ex)
    {
        if (ex == null) { return ""; }
        return ex.InnerException == null
                ? ex.Message
                : ex.Message + " --> " + GetFullError(ex.InnerException);
    }
#endregion

}
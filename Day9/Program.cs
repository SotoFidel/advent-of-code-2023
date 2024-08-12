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

        switch (args[0])
        {
            case "Part1":
                Part1(input);
                break;
            case "Part2":
                Part2(input);
                break;
            default:
                Console.WriteLine("Usage: [Part1|Part2] [FileName]");
                return;
        }
    }

    static void Part1(string input)
    {
        List<List<int>> readings = input
                                    .Split("\r\n")
                                    .Select(l => l.Split(' ')
                                                    .Select(e => int.Parse(e))
                                                    .ToList()
                                    ).ToList();
        
        int extrapolatedValuesSum = 0;

        foreach(List<int> readingSet in readings)
        {
            // readingSet.Add(readingSet.Last() + Process(readingSet));
            extrapolatedValuesSum += readingSet.Last() + Process(readingSet);
        }

        Console.WriteLine("Sum of extrapolated values: " + extrapolatedValuesSum);

        return;
    }

    static void Part2(string input)
    {

    }


    static int Process(List<int> readings)
    {
        List<int> differences = [];
        for(int i = 0; i < readings.Count - 1; i++)
        {
            differences.Add(readings[i+1] - readings[i]);
        }

        if(!differences.All(d => d == 0))
        {
            differences.Add(differences.Last() + Process(differences));
        }

        return differences.Last();
    }

#region utils
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
class Program
{
    static void Main(string[] args)
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
        (StreamReader? streamReader,Exception? exception) = OpenFile(fileName);
        
        if(exception != null)
        {
            Console.WriteLine(GetFullError(exception));
        }

        List<string>lines = streamReader!.ReadToEnd().Split("\r\n")
                            .Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

        int[] LRList = lines[0].Select(c => c == 'L' ? 0 : 1).ToArray();
        lines.RemoveAt(0);
        Dictionary<string,string[]> map = [];

        string[] s;
        string position = "AAA";
        int lrIndex = 0;
        int stepsTaken = 0;
        for(int line = 0; line < lines.Count; line++)
        {
            s = lines[line].Replace("(","").Replace(")","").Replace(" ","").Split(['=',',']);
            if(!map.ContainsKey(s[0]))
            {
                map.Add(s[0],[s[1],s[2]]);
            }
        }

        while(position != "ZZZ")
        {
            position = map[position][LRList[lrIndex]];
            lrIndex++;
            stepsTaken++;
            if (lrIndex == LRList.Length)
            {
                lrIndex = 0;
            }
        }

        Console.WriteLine("Steps: " + stepsTaken);

        return;
    }

    static void Part2(string fileName)
    {


        (StreamReader? streamReader,Exception? exception) = OpenFile(fileName);
        
        if(exception != null)
        {
            Console.WriteLine(GetFullError(exception));
        }

        List<string>lines = streamReader!.ReadToEnd().Split("\r\n")
                            .Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

        int[] LRList = lines[0].Select(c => c == 'L' ? 0 : 1).ToArray();
        lines.RemoveAt(0);
        Dictionary<string,string[]> map = [];

        string[] s;
        List<string> positions = [];
        List<long> stepsPerPosition = [];
        int lrIndex;
        for(int line = 0; line < lines.Count; line++)
        {
            s = lines[line].Replace("(","").Replace(")","").Replace(" ","").Split(['=',',']);

            map.Add(s[0],[s[1],s[2]]);

            if(s[0].EndsWith('A'))
            {
                positions.Add(s[0]);
                stepsPerPosition.Add(0);
            }
        }

        for(int i = 0; i < positions.Count; i++)
        {
            lrIndex = 0;
            while(!positions[i].EndsWith('Z'))
            {
                positions[i] = map[positions[i]][LRList[lrIndex]];
                lrIndex++;
                stepsPerPosition[i]++;
                if (lrIndex == LRList.Length)
                {
                    lrIndex = 0;
                }
            }

            Console.WriteLine("Steps: " + stepsPerPosition[i]);
        }

        Console.WriteLine("Result: " + LeastCommonMultiple(stepsPerPosition));

        return;
    }

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

    static long LeastCommonMultiple(List<long> numbers) =>
        numbers.Count == 0 ? 0 : numbers.Aggregate(ComputeLeastCommonMultiple);

    static long ComputeLeastCommonMultiple(long a, long b) =>
        a * b / GreatestCommonDivisor(a,b);

    static long GreatestCommonDivisor(long a, long b) =>
        b == 0 ? a : GreatestCommonDivisor(b, a % b);
}
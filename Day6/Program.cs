using System.Linq.Expressions;
using System.Net.Http.Headers;

class Program
{

    class Race(long time, long distance)
    {
        public long Time { get; set; } = time;
        public long Distance { get; set; } = distance;
    }

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
        if (exception != null)
        {
            Console.WriteLine(GetFullError(exception));
            return;
        }

        int[][] lines = streamReader!
                        .ReadToEnd()
                        .Split('\n')
                        .Select(r =>
                                r.Split(' ')[1..]
                                .Where(l => !string.IsNullOrEmpty(l))
                                .Select(rc => int.Parse(rc))
                                .ToArray()
                            ).ToArray();

        List<Race> races = [];
        for(int i = 0; i < lines[0].Length; i++)
        {
            races.Add(new(lines[0][i],lines[1][i]));
        }

        long result = 1;
        long waysToWin;
        long totalDistance;
        foreach(Race race in races)
        {
            waysToWin = 0;
            for(int holdTime = 0; holdTime < race.Time; holdTime++)
            {
                if(holdTime == 0 || holdTime == race.Time - 1){continue;}
                totalDistance = holdTime * (race.Time - holdTime);
                if(totalDistance > race.Distance)
                {
                    waysToWin++;
                }
            }

            result *= waysToWin > 0 ? waysToWin : 1;
        }

        Console.WriteLine("Product of ways to win per race: " + result);
        return;

    }

    static void Part2(string fileName)
    {
        (StreamReader? streamReader,Exception? exception) = OpenFile(fileName);
        if (exception != null)
        {
            Console.WriteLine(GetFullError(exception));
            return;
        }

        long[] lines =  streamReader!
                        .ReadToEnd()
                        .Split('\n')
                        .Select(r =>
                                long.Parse(string.Join(null,r.Split(' ')[1..].Where(l => !string.IsNullOrEmpty(l))))            
                        ).ToArray();
        Race race = new(lines[0],lines[1]);

        long waysToWin = 0;
        long totalDistance;
        for(long holdTime = 0; holdTime < race.Time; holdTime++)
        {
            if(holdTime == 0 || holdTime == race.Time - 1){continue;}
            totalDistance = holdTime * (race.Time - holdTime);
            if(totalDistance > race.Distance)
            {
                waysToWin++;
            }
        }

        Console.WriteLine("Total ways to win: " + waysToWin);
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

}
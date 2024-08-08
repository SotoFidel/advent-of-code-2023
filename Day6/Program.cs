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
        (StreamReader? streamReader,Exception? exception) ioResult = OpenFile(fileName);
        if (ioResult.exception != null)
        {
            Console.WriteLine(GetFullError(ioResult.exception));
            return;
        }

        int[][] lines = ioResult.streamReader!.ReadToEnd().Split('\n').Select(r => {
            List<string> record = r.Split(' ').ToList();
            record.RemoveAt(0);
            record.RemoveAll(e => string.IsNullOrEmpty(e));
            return record.Select(rc => int.Parse(rc)).ToArray();
            }).ToArray();

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

            result *= waysToWin;
        }

        Console.WriteLine("Product of ways to win per race: " + result);
        return;

    }

    static void Part2(string fileName)
    {
        (StreamReader? streamReader,Exception? exception) ioResult = OpenFile(fileName);
        if (ioResult.exception != null)
        {
            Console.WriteLine(GetFullError(ioResult.exception));
            return;
        }

        long[] lines = ioResult.streamReader!.ReadToEnd().Split('\n').Select(r => {
            List<string> record = r.Split(' ').ToList();
            record.RemoveAt(0);
            record.RemoveAll(e => string.IsNullOrEmpty(e));
            return long.Parse(string.Join(null,record));
            }).ToArray();

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

    static (StreamReader? streamReader, Exception? exception) OpenFile(string fileName)
    {
        try
        {
            StreamReader streamReader = new StreamReader(fileName);
            return (streamReader, null);
        }
        catch (Exception e)
        {
            return (null, e);
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
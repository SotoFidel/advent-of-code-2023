namespace Day4Part2;

class Program
{
    static void Main(string[] args)
    {
        using StreamReader file = new(args[0]);
        string[] lines = file.ReadToEnd().Split('\n');
        string[] lineSplit;

        List<string> left = new();
        List<string> right = new();
        
        int[] copiesPerCard = new int[lines.Length];
        
        int totalScratchCards = lines.Length;
        int currentTotalMatches = 0;

        for(int i = 0; i < copiesPerCard.Length; i++)
        {
            copiesPerCard[i] = 1;
        }

        for (int currentCardIndex = 0; currentCardIndex < lines.Length; currentCardIndex++)
        {
            lineSplit = lines[currentCardIndex].Split(':')[1].Trim().Split('|');
            
            left = lineSplit[0].Trim().Replace("  "," ").Split(' ').ToList();
            right = lineSplit[1].Trim().Replace("  "," ").Split(' ').ToList();

            for (int evalTimes = 0; evalTimes < copiesPerCard[currentCardIndex]; evalTimes++)
            {
                for(int rightNumberIndex = 0; rightNumberIndex < right.Count; rightNumberIndex++)
                {
                    if( left.Any(l => l == right[rightNumberIndex]) )
                    {
                        currentTotalMatches++;
                        copiesPerCard[currentCardIndex + currentTotalMatches]++;
                        totalScratchCards++;
                    }
                }

                currentTotalMatches = 0;
            }

        }

        Console.WriteLine(totalScratchCards);

    }
}

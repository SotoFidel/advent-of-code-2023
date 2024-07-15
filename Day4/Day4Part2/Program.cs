namespace Day4Part2;

class Card
{
    public int numberOfCopies { get; set; } = 0;
}

class Program
{
    static void Main(string[] args)
    {
        StreamReader file = new StreamReader(args[0]);
        string[] lines = file.ReadToEnd().Split('\n');
        string[] lineSplit;
        List<string> left = new List<string>();
        List<string> right = new List<string>();
        Card[] copies = new Card[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Split(':')[1];
            lineSplit = lines[i].Split('|');
            left = lineSplit[0].Split(' ').ToList();
            right = lineSplit[1].Split(' ').ToList();
            left.RemoveAll(l => String.IsNullOrWhiteSpace(l));
            right.RemoveAll(r => String.IsNullOrWhiteSpace(r));
            for (int j = 0; j < left.Count; j++)
            {
                left[j] = left[j].Trim();
            }
            for (int j = 0; j < right.Count; j++)
            {
                right[j] = right[j].Trim();
            }

        }
    }
}

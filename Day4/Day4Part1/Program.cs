namespace Day4Part1;

class Program
{
    static void Main(string[] args)
    {
        StreamReader file = new StreamReader(args[0]);
        string[] lines = file.ReadToEnd().Split('\n');
        string[] lineSplit;
        List<string>left = new List<string>();
        List<string>right = new List<string>();
        double currentLinePoints = 0;
        double totalPoints = 0;
        for (int i = 0; i < lines.Length; i++){
            lines[i] = lines[i].Split(':')[1];
            lineSplit = lines[i].Split('|');
            left = lineSplit[0].Split(' ').ToList();
            right = lineSplit[1].Split(' ').ToList();
            left.RemoveAll(l => String.IsNullOrWhiteSpace(l));
            right.RemoveAll(r => String.IsNullOrWhiteSpace(r));
            for(int j = 0; j < left.Count; j++){
                left[j] = left[j].Trim();
            }
            for (int j = 0; j < right.Count; j++){
                right[j] = right[j].Trim();
                if (left.Any(l => l == right[j])){
                    currentLinePoints = currentLinePoints == 0 ? 1 : currentLinePoints * 2;
                }
            }
            Console.WriteLine($"Card {i+1} is worth {currentLinePoints} points");
            totalPoints += currentLinePoints;
            currentLinePoints = 0;
        }
        Console.WriteLine($"Total points is {totalPoints}");
    }
}

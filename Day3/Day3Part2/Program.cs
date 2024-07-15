namespace Day3Part2;

class Gear(int xCoord,int yCoord){
    public int X {get;set;} = xCoord;
    public int Y { get; set; } = yCoord;
    public int AdjacentNumbers { get; set; } = 0;
    public List<int> AdjacentNumbersList {get;set;} = new List<int>();
}

class Program
{
    static List<Gear>gears = new List<Gear>();
    static Gear? gearRef;

    static void Main(string[] args)
    {
        if (args.Length < 1) {
            Console.WriteLine("Please provide file name");
        }

        StreamReader file = new StreamReader(args[0]);
        // string input = file.ReadToEnd();
        string[] lines = file.ReadToEnd().Split('\n');
        List<char[]> chars = new List<char[]>();
        List<int> partNumbers = new List<int>();
        string currentNumberString = "";
        int partNumberSum = 0;
        int skip = 0;
        bool isPartNumber = false;
        int gearRatioSum = 0;

        foreach(string line in lines){
            chars.Add(line.ToCharArray());
        }

        for(int i = 0; i < chars.Count; i++){
            for(int j = 0; j < chars[i].Length; j++){
                if(char.IsNumber(chars[i][j])){
                    for(int k = j; k < chars[i].Length && char.IsNumber(chars[i][k]); k++){
                        currentNumberString += chars[i][k];
                        skip++;

                        // Character to the left
                        isPartNumber = isPartNumber ? true : (k-1 >= 0 && validSymbol(chars[i][k-1],i,k-1));

                        // Character to the right
                        isPartNumber = isPartNumber ? true : (k+1 < chars[i].Length && validSymbol(chars[i][k+1],i,k+1));

                        
                        // Characters below
                        if ( (i + 1) < chars.Count){
                            // Character straight down
                            isPartNumber = isPartNumber ? true : validSymbol(chars[i+1][k],i+1,k);
                            
                            // Character diagonal down left
                            isPartNumber = isPartNumber ? true : (k-1 >= 0 && validSymbol(chars[i+1][k-1],i+1,k-1));

                            // Character diagonal dowwn right
                            isPartNumber = isPartNumber ? true : (k+1 < chars[i].Length && validSymbol(chars[i+1][k+1],i+1,k+1));

                        }

                        // Characters above
                        if (i > 0){

                            // Character straight up
                            isPartNumber = isPartNumber ? true : validSymbol(chars[i-1][k],i-1,k);

                            // Character diagonal up left
                            isPartNumber = isPartNumber ? true : (k-1 >= 0 && validSymbol(chars[i-1][k-1],i-1,k-1));

                            // Character diagonal up right
                            isPartNumber = isPartNumber ? true : (k+1 < chars[i].Length && validSymbol(chars[i-1][k+1],i-1,k+1));

                        }


                    }

                    j += skip;
                }
                if (isPartNumber){
                    partNumberSum += int.Parse(currentNumberString);
                    gearRef?.AdjacentNumbersList.Add(int.Parse(currentNumberString));
                    
                }
                
                isPartNumber = false;
                currentNumberString = "";
                skip = 0;
            }
        }

        Console.WriteLine(partNumberSum);
        gears = gears.OrderBy(g => g.X).ThenBy(g => g.Y).ToList();
        foreach(Gear g in gears){
            Console.WriteLine($"Gear [{g.X}][{g.Y}]: AdjNums:{g.AdjacentNumbers}");
            foreach(int n in g.AdjacentNumbersList){
                Console.WriteLine($"\t[{n}]");
            }
            if (g.AdjacentNumbersList.Count == 2){
                gearRatioSum += g.AdjacentNumbersList[0] * g.AdjacentNumbersList[1];
            }
        }

        Console.WriteLine(gearRatioSum);

        
    }

    // Should only get called if isPartNumber is false.
    // 
    static bool validSymbol(char c, int x, int y){
        if (c == '*'){
            if (!gears.Exists(g => g.X == x && g.Y == y)){
                gearRef = new Gear(x,y);
                gears.Add(gearRef);
            }
            gearRef = gears.First(g => g.X == x && g.Y == y);
            gearRef.AdjacentNumbers++;
            return true;
        }
        return false;
    }
}

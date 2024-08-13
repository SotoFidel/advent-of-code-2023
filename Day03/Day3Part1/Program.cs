using System.Security.Cryptography;
using System.Security.Principal;

namespace Day3Part1;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1) {
            Console.WriteLine("Please provide file name");
        }

        StreamReader file = new StreamReader(args[0]);
        string input = file.ReadToEnd();
        string[] lines = input.Split('\n');
        List<char[]> chars = new List<char[]>();
        List<int> partNumbers = new List<int>();
        string currentNumberString = "";
        int partNumberSum = 0;
        int skip = 0;
        bool isPartNumber = false;

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
                        isPartNumber = isPartNumber || (k-1 >= 0 && validSymbol(chars[i][k-1]));

                        // Character to the right
                        isPartNumber = isPartNumber || (k+1 < chars[i].Length && validSymbol(chars[i][k+1]));
                        
                        // Characters below
                        if ( (i + 1) < chars.Count && !isPartNumber){
                            // Character straight down
                            isPartNumber = isPartNumber || validSymbol(chars[i+1][k]);
                            
                            // Character diagonal down left
                            isPartNumber = isPartNumber || (k-1 >= 0 && validSymbol(chars[i+1][k-1]));

                            // Character diagonal dowwn right
                            isPartNumber = isPartNumber || (k+1 < chars[i].Length && validSymbol(chars[i+1][k+1]));

                        }

                        // Characters above
                        if (i > 0 && !isPartNumber){

                            // Character straight up
                            isPartNumber = isPartNumber || validSymbol(chars[i-1][k]);

                            // Character diagonal up left
                            isPartNumber = isPartNumber || (k-1 >= 0 && validSymbol(chars[i-1][k-1]));

                            // Character diagonal up right
                            isPartNumber = isPartNumber || (k+1 < chars[i].Length && validSymbol(chars[i-1][k+1]));

                        }


                    }
                    Console.WriteLine("Found number: " + currentNumberString);
                    j += skip;
                }
                if (isPartNumber){
                    partNumberSum += int.Parse(currentNumberString);
                    // Console.WriteLine("Part number " + currentNumberString + " is valid");
                }
                
                isPartNumber = false;
                currentNumberString = "";
                skip = 0;
            }
        }

        Console.WriteLine(partNumberSum);

        
    }

    static bool validSymbol(char c){
        if ((int)c != 46 && ((int)c < 48 || (int)c > 57)){
            Console.WriteLine("Characterr " + c + " is valid");
        }
        return (int)c != 46 && ((int)c < 48 || (int)c > 57);
    }
}

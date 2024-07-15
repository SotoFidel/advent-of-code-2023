using System.IO;
using System.Numerics;

namespace Day1;

class Program
{
    static void Main(string[] args)
    {
        if(args.Length != 2)
        {
            Console.WriteLine("Provide 2 arguments ([partNumber] [fileName]):\n\tPart1: 1 [fileName]\n\tPart2: 2 [fileName]");
            return;
        }


        switch(args[0])
        {
            case "1":
                Part1(args[1]);
                break;
            case "2":
                Part2(args[1]);
                break;
            default: 
                Console.WriteLine("Provide 2 arguments ([partNumber] [fileName]):\n\tPart1: 1 [fileName]\n\tPart2: 2 [fileName]");
                return;
        }
    }


    static void Part1(string fileName)
    {
        StreamReader? streamReader;
        if(!OpenFile(out streamReader,fileName))
        {
            return;
        }

        int sum = 0;
        string currentNumber = "";
        string? line = streamReader?.ReadLine();

        while (line != null)
        {

            for(int i = 0; i < line.Length; i++)
            {
                // Ascii 0-9 == 48-57
                if(line[i] >= 48 && line[i] <= 57)
                {
                    currentNumber += line[i];
                }
            }

            currentNumber = "" + currentNumber[0] + currentNumber[currentNumber.Length - 1];

            sum += int.Parse(currentNumber);

            currentNumber = "";
            line = streamReader?.ReadLine();
        }

        Console.WriteLine("Sum: " + sum);
    }

    static void Part2(string fileName)
    {
        Dictionary<string,int> numberDict = new Dictionary<string, int>(){
            {"zero",0},     {"0",0},
            {"one",1},      {"1",1},
            {"two",2},      {"2",2},
            {"three",3},    {"3",3},
            {"four",4},     {"4",4},
            {"five",5},     {"5",5},
            {"six",6},      {"6",6},
            {"seven",7},    {"7",7},
            {"eight",8},    {"8",8},
            {"nine",9},     {"9",9}
        };

        StreamReader? streamReader;
        if (!OpenFile(out streamReader, fileName))
        {
            return;
        }

        string? line = streamReader?.ReadLine();
        string currentNumber = "";
        string currentKey;
        int sum = 0;;
        int stringRemainingLength;
        while(line != null)
        {
            stringRemainingLength = line.Length;

            for(int i = 0; i < line.Length; i++)
            {
                for (int j = 1; j <= stringRemainingLength; j++)
                {
                    
                    currentKey = line.Substring(i,j);

                    if (numberDict.ContainsKey(currentKey)){
                        currentNumber += numberDict[currentKey];
                        break;
                    }

                }

                stringRemainingLength--;
            }

            currentNumber = "" + currentNumber[0] + currentNumber[currentNumber.Length - 1];
            sum += int.Parse(currentNumber);
            
            currentNumber = "";
            line = streamReader?.ReadLine();
        }

        Console.WriteLine(sum);

    }

    static bool OpenFile(out StreamReader? streamReader,string fileName)
    {
        try
        {
            streamReader = new(fileName);
            return true;
        }
        catch(Exception)
        {
            Console.WriteLine("Couldn't load file");
            streamReader = null;
            return false;
        }

    }
}

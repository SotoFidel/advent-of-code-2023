namespace Day1;

class Digit {
    public int Value {get; set; } = 0;
    public bool IsSet { get; set; } = false;
}

class Program
{
    static void Main(string[] args)
    {
        int calibrationValueSum = 0;

        Digit firstDigit = new Digit();
        Digit secondDigit = new Digit();

        Dictionary<string,int> numberDict = new Dictionary<string, int>(){
            {"zero",0},     {"0",0},
            {"one",1},      {"1",1},
            {"two",2},      {"2",2},
            {"three",3},    {"3",3},
            {"four",4},     {"4",4},
            {"five",5},     {"5",5},
            {"6",6},        {"six",6},
            {"7",7},        {"seven",7},
            {"8",8},        {"eight",8},
            {"9",9},        {"nine",9}
        };
        
        StreamReader streamReader = new StreamReader("input.txt");
        char[]? line;
        string currentEval = "";
        int stringRemainingLength = 0;
        int currentLine = 1;
        string lineString = "";


        while ((line = streamReader.ReadLine()?.ToCharArray()) != null) {

            lineString = new string(line);
            stringRemainingLength = lineString.Length;
            currentEval = "";


            // Left to right for second digit. Eval indiscriminately for second digit.
            for (int i = 0; i < lineString.Length; i++){

                for (int j = 1; j <= stringRemainingLength; j++){
                    
                    currentEval = lineString.Substring(i,j);

                    if (numberDict.ContainsKey(currentEval)){
                        Console.WriteLine("Line: " + currentLine + "\t2nd Digit match. currentEval = " + currentEval);
                        if (!firstDigit.IsSet){
                            firstDigit.Value = numberDict[currentEval];
                            firstDigit.IsSet = true;
                        }
                        secondDigit.Value = numberDict[currentEval];
                        secondDigit.IsSet = true;
                        break;
                    }

                }

                stringRemainingLength--;

            }

            calibrationValueSum += int.Parse($"{firstDigit.Value}{secondDigit.Value}");
            Console.WriteLine($"Line Result: {firstDigit.Value}{secondDigit.Value}");

            firstDigit.IsSet = false;
            firstDigit.Value = 0;
            secondDigit.IsSet = false;
            secondDigit.Value = 0;
            currentLine++;

        }

        Console.WriteLine(calibrationValueSum);

    }
}

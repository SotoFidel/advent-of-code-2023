/*
    Integrates both part 1 and part 2 to the solution. 
*/

namespace Day2;

class Program
{
    static void Main(string[] args)
    {
        int redCubes = 12;
        int greenCubes = 13;
        int blueCubes = 14;
        int minRedCubesNeeded,minGreenCubesNeeded,minBlueCubesNeeded;
            minRedCubesNeeded = minGreenCubesNeeded = minBlueCubesNeeded = 0;
        int gameIdSum = 0;
        int currentGameId = -1;
        bool gameIsPossible;
        int total = 0;

        StreamReader streamReader = new StreamReader("input.txt");
        string? line;
        string[] gameSets;
        string[] gameSet;
        string[] game;

        while ((line = streamReader.ReadLine()) != null)
        {

            gameIsPossible = true;
            minRedCubesNeeded = minGreenCubesNeeded = minBlueCubesNeeded = 0;
            

            // Game n: x blue, y red, z green; x blue, z green
            gameSets = line.Split(':', ';');
            currentGameId = int.Parse(gameSets[0].Split(' ')[1]);
            for (int i = 1; i < gameSets.Length; i++)
            {

                gameSet = gameSets[i].Split(',');
                for (int j = 0; j < gameSet.Length; j++)
                {
                    game = gameSet[j].Trim().Split(' ');
                    switch (game[1])
                    {
                        case "red":
                            if (redCubes < int.Parse(game[0])) { gameIsPossible = false; }
                            if (minRedCubesNeeded < int.Parse(game[0])) { minRedCubesNeeded = int.Parse(game[0]); }
                            break;
                        case "blue":
                            if (blueCubes < int.Parse(game[0])) { gameIsPossible = false; }
                            if (minBlueCubesNeeded < int.Parse(game[0])) { minBlueCubesNeeded = int.Parse(game[0]); }
                            break;
                        case "green":
                            if (greenCubes < int.Parse(game[0])) { gameIsPossible = false; }
                            if (minGreenCubesNeeded < int.Parse(game[0])) { minGreenCubesNeeded = int.Parse(game[0]); }
                            break;
                        default:
                            Console.WriteLine("Catastrophy");
                            break;
                    }
                }


            }
            
            total += minRedCubesNeeded * minGreenCubesNeeded * minBlueCubesNeeded;

            if (gameIsPossible) 
            {
                gameIdSum += currentGameId;
            }

        }

        Console.WriteLine("Sum of viable game IDs: " + gameIdSum);
        Console.WriteLine("Sum of powwers of minimum cubes needed: " + total);

    }
}






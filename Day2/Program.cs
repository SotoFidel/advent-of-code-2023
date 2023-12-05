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

        StreamReader streamReader = new StreamReader("input.txt");
        string? line;
        string[] gameSets;
        string[] gameSet;
        string[] game;

        while ((line = streamReader.ReadLine()) != null)
        {

            gameIsPossible = true;
            

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
                            break;
                        case "blue":
                            if (blueCubes < int.Parse(game[0])) { gameIsPossible = false; }
                            break;
                        case "green":
                            if (greenCubes < int.Parse(game[0])) { gameIsPossible = false; }
                            break;
                        default:
                            Console.WriteLine("Catastrophy");
                            break;
                    }
                }


            }

            if (gameIsPossible) {
                gameIdSum += currentGameId;
            }

        }

        Console.WriteLine(gameIdSum);

    }
}






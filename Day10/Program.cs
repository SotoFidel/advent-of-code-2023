class Program
{

    class Coordinate(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };

    static Dictionary<char,Dictionary<Direction,Direction>> pipes = new()
    {
        {'|',new(){
            {Direction.Up,Direction.Up},
            {Direction.Down,Direction.Down}
        }},
        {'-',new(){
            {Direction.Right,Direction.Right},
            {Direction.Left,Direction.Left}
        }},
        {'L',new(){
            {Direction.Down,Direction.Right},
            {Direction.Left,Direction.Up}
        }},
        {'J',new(){
            {Direction.Right,Direction.Up},
            {Direction.Down,Direction.Left}
        }},
        {'7',new(){
            {Direction.Right,Direction.Down},
            {Direction.Up,Direction.Left}
        }},
        {'F',new(){
            {Direction.Up,Direction.Right},
            {Direction.Left,Direction.Down}
        }},
    };

    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: [Part1|Part2] [FileName]");
            return;
        }

        (StreamReader? streamReader, Exception? exception) = OpenFile(args[1]);
        if(exception != null)
        {
            Console.WriteLine(GetFullError(exception));
            return;
        }

        string input = streamReader!.ReadToEnd();
        streamReader.Close();

        string[] lines = input.Split("\r\n");
        
        switch (args[0])
        {
            case "Part1":
                    Part1(lines);
                break;
            case "Part2":
                    Part2(lines);
                break;
            default:
                Console.WriteLine("Usage: [Part1|Part2] [FileName]");
                return;
        }
    }

    static void Part1(string[] lines)
    {

        (Coordinate coord,Direction direction,_) = GetStartPositionAndDirection(lines);
        
        int stepsTaken = 0;
        while(true) {

            coord.Y += direction switch
            {
                Direction.Up => -1,
                Direction.Down => 1,
                _ => 0
            };

            coord.X += direction switch
            {
                Direction.Left => -1,
                Direction.Right => 1,
                _ => 0
            };

            stepsTaken++;
            if(lines[coord.Y][coord.X] == 'S') { break; }

            direction = pipes[lines[coord.Y][coord.X]][direction];
        }

        Console.WriteLine("StepsTaken: " + stepsTaken);
        Console.WriteLine("Res: " + stepsTaken / 2);
        
    }

    static void Part2(string[] lines)
    {
        (Coordinate coord,Direction direction,bool startIsVertex) = GetStartPositionAndDirection(lines);
        List<Coordinate> vertices = [];
        if(startIsVertex)
        {
            vertices.Add(new(coord.X,coord.Y));
        }

        int points = 0;
        while(true) {

            coord.Y += direction switch
            {
                Direction.Up => -1,
                Direction.Down => 1,
                _ => 0
            };

            coord.X += direction switch
            {
                Direction.Left => -1,
                Direction.Right => 1,
                _ => 0
            };

            points++;
            if(lines[coord.Y][coord.X] == 'S') { break; }
            if("LJF7".Contains(lines[coord.Y][coord.X]))
            {
                vertices.Add(new(coord.X,coord.Y));
            }
            direction = pipes[lines[coord.Y][coord.X]][direction];
        }

        long Area = 0;


        // Shoelace algorithm to determine area encased by the pipes.
        for(int vertex = 0; vertex < vertices.Count - 1; vertex++)
        {
            Area += (vertices[vertex].X * vertices[vertex + 1].Y) - (vertices[vertex+1].X * vertices[vertex].Y);
        }
        Area += (vertices.Last().X * vertices.First().Y) - (vertices.First().X * vertices.Last().Y);
        Area = Math.Abs((long)Math.Ceiling((double)Area / 2));

        // Pick's theorem moved around a bit to solve for i (number of dots in area)
        // Instead of A = i + (b /  2) - 1
        // we have i = (-b / 2) + 1 + A
        long i = (-points / 2) + 1 +  Area;
        Console.WriteLine("i: " + i);


    }


static (Coordinate coord,Direction direction,bool isVertex) GetStartPositionAndDirection(string[] lines)
{
    Coordinate coord = new(0,0);
    bool found = false;
    for(int line = 0; line < lines.Length; line++)
    {
        for(int character = 0; character < lines[line].Length; character++)
        {
            if(lines[line][character] == 'S')
            {
                coord.X = character;
                coord.Y = line;
                found = true;
                Console.WriteLine("Found S at position " + coord.X + "," + coord.Y);
                break;
            }
        }

        if (found) { break; }
    }

    Direction direction = Direction.Left;
    bool incomingVertical = false;
    bool incomingHorizontal = false;

    // Top
    if(coord.Y - 1 >= 0 && "|7F".Contains(lines[coord.Y - 1][coord.X]))
    {
        direction = Direction.Up;
        incomingVertical = true;
    }

    // Right
    if(coord.X + 1 < lines[coord.Y].Length && "-J7".Contains(lines[coord.Y][coord.X + 1]))
    {
        direction = Direction.Right;
        incomingHorizontal = true;
    }

    // Left
    if(coord.X - 1 >= 0 && "-LF".Contains(lines[coord.Y][coord.X - 1]))
    {
        direction = Direction.Left;
        incomingHorizontal = true;
    }

    // Bottom
    if(coord.Y + 1 < lines.Length && "|LJ".Contains(lines[coord.Y + 1][coord.X]))
    {
        direction = Direction.Down;
        incomingVertical = true;
    }
    
    return (coord,direction,incomingHorizontal && incomingVertical);

}


#region Utils
    static Tuple<StreamReader?,Exception?> OpenFile(string fileName)
    {
        try
        {
            StreamReader streamReader = new StreamReader(fileName);
            return new (streamReader, null);
        }
        catch (Exception e)
        {
            return new (null, e);
        }
    }

    static string GetFullError(Exception? ex)
    {
        if (ex == null) { return ""; }
        return ex.InnerException == null
                ? ex.Message
                : ex.Message + " --> " + GetFullError(ex.InnerException);
    }
#endregion

}



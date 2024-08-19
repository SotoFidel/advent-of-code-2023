using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;


class Program
{

    class Coord(long x, long y)
    {
        public long X { get; set; } = x;
        public long Y { get; set; } = y;
    }

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

        List<string> lines = [.. input.Split("\r\n")];
        
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


    static void Part1(List<string> lines)
    {

        int lineWidth = lines[0].Length;
        // Expansion row by row
        for(int line = 0; line < lines.Count; line++)
        {
            if(!lines[line].Contains('#'))
            {
                lines.Insert(line,"");
                for(int i = 0; i < lineWidth; i++)
                {
                    lines[line] += '.';
                }
                line++;
            }
        }

        // Expansion column by column
        string columnString;
        for(int column = 0; column < lineWidth; column++)
        {
            columnString = "";
            for(int line = 0; line < lines.Count; line++)
            {
                columnString += lines[line][column];
            }
            if(!columnString.Contains('#'))
            {
                for(int line = 0; line < lines.Count; line++)
                {
                    lines[line] = lines[line].Insert(column,"."); 
                }
                column++;
                lineWidth++;
            }
        }

        List<Coord> galaxies = [];
        List<long> lengths = [];
        for(int line = 0; line < lines.Count; line++)
        {
            for(int character = 0; character < lines[line].Length; character++)
            {
                if(lines[line][character] == '#')
                {
                    galaxies.Add(new(character,line));
                }
            }
        }

        for (int i = 0; i < galaxies.Count; i++)
        {
            for(int j = i + 1; j < galaxies.Count; j++)
            {
                lengths.Add(Math.Abs(galaxies[j].X - galaxies[i].X) + Math.Abs(galaxies[j].Y - galaxies[i].Y));
            }
        }

        Console.WriteLine(lengths.Sum());

        return;
    }

    static void Part2(List<string> lines)
    {
        int lineWidth = lines[0].Length;
        List<int> emptyRows = [];
        List<int> emptyColumns = [];
        // Expansion row by row
        for(int line = 0; line < lines.Count; line++)
        {
            if(!lines[line].Contains('#'))
            {
                emptyRows.Add(line);
            }
        }

        // Expansion column by column
        string columnString;
        for(int column = 0; column < lineWidth; column++)
        {
            columnString = "";
            for(int line = 0; line < lines.Count; line++)
            {
                columnString += lines[line][column];
            }
            if(!columnString.Contains('#'))
            {
                emptyColumns.Add(column);
            }
        }

        List<Coord> galaxies = [];
        List<long> lengths = [];
        for(int line = 0; line < lines.Count; line++)
        {
            for(int character = 0; character < lines[line].Length; character++)
            {
                if(lines[line][character] == '#')
                {
                    Coord galaxyToAdd = new(character,line);
                    for(int i = 0; i < emptyRows.Count; i++)
                    {
                        if(emptyRows[i] < line)
                        {
                            galaxyToAdd.Y += (1000000 - 1);
                        }
                    }
                    for(int i = 0; i < emptyColumns.Count; i++)
                    {
                        if(emptyColumns[i] < character)
                        {
                            galaxyToAdd.X += (1000000 - 1);
                        }
                    }
                    galaxies.Add(galaxyToAdd);
                }
            }
        }

        for (int i = 0; i < galaxies.Count; i++)
        {
            for(int j = i + 1; j < galaxies.Count; j++)
            {
                lengths.Add(Math.Abs(galaxies[j].X - galaxies[i].X) + Math.Abs(galaxies[j].Y - galaxies[i].Y));
            }
        }

        Console.WriteLine(lengths.Sum());

        return;
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

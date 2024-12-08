using System.Numerics;

var fileName = Path.Combine(Environment.CurrentDirectory, "Input.txt");

if (!File.Exists(fileName))
{
    Console.Error.WriteLine("Provided input file does not exist.");
    return;
}

var input = string.Empty;

var wordGrid = PopulateListFromInput(fileName);

var totalMatches = SolveWordGrid(wordGrid);

Console.WriteLine("Number of matches: " + totalMatches);
Console.WriteLine("Press any key to exit.");
Console.ReadKey();

static int SolveWordGrid(char[,] wordGrid)
{
    List<Vector2> vectors = new List<Vector2>
    {
        new Vector2(-1, -1),
        new Vector2(-1, 1),
        new Vector2(1, -1),
        new Vector2(1, 1)
    };

    int totalMatches = 0;
    for (int i = 0; i < Math.Pow(wordGrid.Length, .5); i++)
    {
        for (int j = 0; j < Math.Pow(wordGrid.Length, .5); j++)
        {
            //First letter is an M
            if (wordGrid[i,j] == 'M')
            {
                //for each diagnonal from this index
                foreach (var diagonal in vectors)
                {
                    if (CheckDiagonal(wordGrid, diagonal, i, j))
                    {
                        totalMatches++;
                    }
                }
            }
        }
    }

    return totalMatches / 2; //All matches are calculated twice
}

static bool CheckDiagonal(char[,] wordGrid, Vector2 diagonal, int x, int y)
{
    try
    {
        //Check diagonal for an A
        x += (int)diagonal.X;
        y += (int)diagonal.Y;

        if (wordGrid[x, y] != 'A')
            return false;

        //Check perpendicular diagonals for an M and and S
        var crossChar1 = wordGrid[x + (int)(-1 * diagonal.X), y + (int)diagonal.Y];
        var crossChar2 = wordGrid[x + (int)diagonal.X, y + (int)(-1 * diagonal.Y)];

        if (crossChar1 == crossChar2 
            || (crossChar1 != 'M' && crossChar1 != 'S')
            || (crossChar2 != 'M' && crossChar2 != 'S'))
        {
            return false;
        }

        //Check contiunation of diagonal for an S
        x += (int)diagonal.X;
        y += (int)diagonal.Y;

        if (wordGrid[x, y] != 'S')
            return false;

        return true;
    }
    catch (IndexOutOfRangeException ex)
    {
        //If we fell out of the grid, return false
        return false;
    }
    
}

static char[,] PopulateListFromInput(string fileName)
{
    char[,] wordGrid = null;

    using (var file = new StreamReader(fileName))
    {
        int lineNumber = 0;

        // Read the file line by line
        while (true)
        {
            var fileLine = file.ReadLine();

            // If the line is empty, we have reached the end of the file
            if (string.IsNullOrEmpty(fileLine))
                break;

            if (wordGrid == null)
            {
                var lineLength = fileLine.Length;
                wordGrid = new char[lineLength, lineLength];
            }

            var lineCharacters = fileLine.ToCharArray();
            for (int i = 0; i < lineCharacters.Length; i++)
            {
                wordGrid[lineNumber, i] = lineCharacters[i];
            }

            lineNumber++;
        }
    }

    return wordGrid;
}
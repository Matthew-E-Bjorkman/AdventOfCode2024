
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

int SolveWordGrid(char[,] wordGrid)
{
    List<Vector2> vectors = new List<Vector2>
    {
        new Vector2(-1, 0),
        new Vector2(-1, -1),
        new Vector2(-1, 1),
        new Vector2(0, -1),
        new Vector2(0, 1),
        new Vector2(1, -1),
        new Vector2(1, 0),
        new Vector2(1, 1)
    };

    int totalMatches = 0;
    for (int i = 0; i < Math.Pow(wordGrid.Length, .5); i++)
    {
        for (int j = 0; j < Math.Pow(wordGrid.Length, .5); j++)
        {
            foreach (var vector in vectors)
            {
                if (CheckLetter(wordGrid[i, j], 0, wordGrid, new Vector2(i, j), vector))
                {
                    totalMatches++;
                }
            }
        }
    }

    return totalMatches;
}

bool CheckLetter(char letter, int letterExpectedIndex, char[,] wordGrid, Vector2 gridIndex, Vector2 vector)
{
    char[] wordToSearch =
    {

        'X','M','A','S'
    };

    var letterExpected = wordToSearch[letterExpectedIndex];

    if (!letter.Equals(letterExpected))
    {
        return false;
    }

    if (letterExpectedIndex == wordToSearch.Length - 1)
    {
        return true;
    }

    gridIndex += vector;
    letterExpectedIndex++;

    if (gridIndex.X < 0 || gridIndex.X >= Math.Pow(wordGrid.Length, .5)
        || gridIndex.Y < 0 || gridIndex.Y >= Math.Pow(wordGrid.Length, .5))
    {
        return false;
    }
       return CheckLetter(wordGrid[(int)gridIndex.X, (int)gridIndex.Y], letterExpectedIndex, wordGrid, gridIndex, vector);
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
                wordGrid = new char[lineLength,lineLength];
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
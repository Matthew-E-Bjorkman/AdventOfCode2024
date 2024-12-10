
using System.Numerics;

var mapGrid = PopulateGridFromInput(out var guardIndex);

var patrolPathLength = SolvePatrolPathGrid(mapGrid, guardIndex);

Console.WriteLine($"The length of the patrol path is: {patrolPathLength}");
Console.WriteLine("");

for (int i = 0; i < mapGrid.GetLength(0); i++)
{
    for (int j = 0; j < mapGrid.GetLength(1); j++)
    {
        Console.Write(mapGrid[i, j]);
    }
    Console.WriteLine("");
}
Console.WriteLine("");

Console.WriteLine("Press any key to exit.");
Console.ReadKey();

static int SolvePatrolPathGrid(char[,] mapGrid, Vector2 guardIndex)
{
    try
    {
        var guardDirection = new Vector2(-1, 0);
        while (true)
        {
            mapGrid[(int)guardIndex.X, (int)guardIndex.Y] = 'X';

            char nextCellInPath = mapGrid[(int)(guardIndex.X + guardDirection.X), (int)(guardIndex.Y + guardDirection.Y)];
            if (nextCellInPath == '#')
            {
                switch (guardDirection)
                {
                    case var direction when direction == new Vector2(0, 1):
                        guardDirection = new Vector2(1, 0);
                        break;
                    case var direction when direction == new Vector2(1, 0):
                        guardDirection = new Vector2(0, -1);
                        break;
                    case var direction when direction == new Vector2(0, -1):
                        guardDirection = new Vector2(-1, 0);
                        break;
                    case var direction when direction == new Vector2(-1, 0):
                        guardDirection = new Vector2(0, 1);
                        break;
                }
            }
            else
            {
                guardIndex += guardDirection;
            }
        }
    }
    catch (IndexOutOfRangeException ex)
    {
        //  The guard tried to exit the grid (tried to move outside of valid indexes)

        int cellsVisited = 0;
        foreach (var cell in mapGrid)
        {
            if (cell.Equals('X'))
            {
                cellsVisited++;
            }
        }

        return cellsVisited;
    }

    // This will never occur, an infinite loop would happen first.
    return 0;
}

static char[,] PopulateGridFromInput(out Vector2 guardIndex)
{
    var fileName = Path.Combine(Environment.CurrentDirectory, "Input.txt");

    if (!File.Exists(fileName))
    {
        Console.Error.WriteLine("Provided input file does not exist.");
        Console.ReadKey();
        Environment.Exit(1);
    }

    char[,] grid = null;
    guardIndex = new Vector2();

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

            if (grid == null)
            {
                var lineLength = fileLine.Length;
                grid = new char[lineLength, lineLength];
            }

            var lineCharacters = fileLine.ToCharArray();
            for (int i = 0; i < lineCharacters.Length; i++)
            {
                if (lineCharacters[i] == '^')
                {
                    guardIndex = new Vector2(lineNumber, i);
                }
                grid[lineNumber, i] = lineCharacters[i];
            }

            lineNumber++;
        }
    }

    return grid;
}
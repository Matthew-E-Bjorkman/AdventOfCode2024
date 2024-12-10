
using System.Numerics;
using System.Threading;

var mapGrid = PopulateGridFromInput(out var guardIndex);

var numberOfPossibleObstructions = SolveLoopsForPatrolPathGrid(mapGrid, guardIndex);

Console.WriteLine($"The number of possible obstructions to create an infinite loop: {numberOfPossibleObstructions}");
Console.WriteLine("Press any key to exit.");
Console.ReadKey();

static int SolveLoopsForPatrolPathGrid(char[,] mapGrid, Vector2 guardIndex)
{
    int numberOfPossibleObstructions = 0;
    for (int i = 0; i < mapGrid.GetLength(0); i++)
    {
        for (int j = 0; j < mapGrid.GetLength(1); j++)
        {
            if (!mapGrid[i, j].Equals('.'))
            {
                continue;
            }

            Console.WriteLine($"Attempting obstruction at: {i}, {j}");

            var tempGrid = (char[,])mapGrid.Clone();
            tempGrid[i, j] = '#';

            var cancellationToken = new CancellationTokenSource();
            var token = cancellationToken.Token;

            var gridSolveAttempt = Task.Run(() => 
            {
                try
                {
                    SolvePatrolPathGrid(tempGrid, guardIndex, token);
                }
                catch (OperationCanceledException ex)
                {
                    numberOfPossibleObstructions++;
                    Console.WriteLine($"Infinite loop detected at: {i}, {j}. New count: {numberOfPossibleObstructions}");
                }
            }, cancellationToken.Token);

            // Check if thread is in an infinite loop
            if (!gridSolveAttempt.Wait(TimeSpan.FromMilliseconds(100)))
            {
                cancellationToken.Cancel();
            }

            cancellationToken.Dispose();

        }
    }

    return numberOfPossibleObstructions;
}

static int SolvePatrolPathGrid(char[,] mapGrid, Vector2 guardIndex, CancellationToken cancellationToken)
{
    try
    {
        var guardDirection = new Vector2(-1, 0);
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
                break;
            }

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
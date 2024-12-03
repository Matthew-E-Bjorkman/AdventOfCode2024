internal class Program
{
    private static void Main(string[] args)
    {
        var fileName = Path.Combine(Environment.CurrentDirectory, "Input.txt");

        if (!File.Exists(fileName))
        {
            Console.Error.WriteLine("Provided input file does not exist.");
            return;
        }

        // Generate the list of reports
        var reportList = PopulateListFromInput(fileName);

        // Calculate the number of safe reports
        var numberOfSafeReports = CalculateSafeReportCount(reportList);

        Console.WriteLine("Number of safe reports: " + numberOfSafeReports);
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    private static int CalculateSafeReportCount(List<List<int>> reportList)
    {
        var safeReportCount = 0;

        foreach (var report in reportList)
        {
            var reportIsSafe = IsReportSafe(report);

            if (!reportIsSafe)
            {
                for (int i = 0; i < report.Count; i++)
                {
                    var modifiedReport = new List<int>(report);
                    modifiedReport.RemoveAt(i);
                    if (IsReportSafe(modifiedReport))
                    {
                        reportIsSafe = true;
                        break;
                    }
                }
            }

            if (reportIsSafe)
                safeReportCount++;
        }

        return safeReportCount;
    }

    private static bool IsReportSafe(List<int> report)
    {
        int lastDelta = 0;
        var reportIsSafe = true;
        for (int i = 0; i < report.Count - 1; i++)
        {
            int currentDelta = report[i] - report[i + 1];

            // First rule: The levels are either all increasing or all decreasing.
            if (lastDelta != 0 && currentDelta * lastDelta <= 0)
            {
                reportIsSafe = false;
                break;
            }

            // Second Rule: Any two adjacent levels differ by at least one and at most three.
            if (currentDelta == 0 || currentDelta > 3 || currentDelta < -3)
            {
                reportIsSafe = false;
                break;
            }

            lastDelta = currentDelta;
        }

        return reportIsSafe;
    }

    private static List<List<int>> PopulateListFromInput(string fileName)
    {
        var reportList = new List<List<int>>();

        using (var file = new StreamReader(fileName))
        {
            // Read the file line by line
            while (true)
            {
                var fileLine = file.ReadLine();

                // If the line is empty, we have reached the end of the file
                if (string.IsNullOrEmpty(fileLine))
                    break;

                // Split the line by spaces into the list of values
                var listValues = fileLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

                // Insert the values into the list
                reportList.Add(listValues.Select(int.Parse).ToList());
            }
        }

        return reportList;
    }
}
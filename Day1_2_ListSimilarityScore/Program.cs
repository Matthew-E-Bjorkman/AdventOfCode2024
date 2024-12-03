
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

        var list1 = new LinkedList<int>();
        var list2 = new LinkedList<int>();

        //Generate the two lists, sorted
        PopulateListsFromInput(list1, list2, fileName);

        // Calculate the total distance between the two lists
        var listSimilarityScore = CalculateListSimilarityScore(list1, list2);

        Console.WriteLine("List similarity score: " + listSimilarityScore);
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    private static void PopulateListsFromInput(LinkedList<int> list1, LinkedList<int> list2, string fileName)
    {
        using (var file = new StreamReader(fileName))
        {
            // Read the file line by line
            while (true)
            {
                var fileLine = file.ReadLine();

                // If the line is empty, we have reached the end of the file
                if (string.IsNullOrEmpty(fileLine))
                    break;

                // Split the line by spaces into the respective value for each list
                var listValues = fileLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

                // Insert the values into their lists in sorted order.
                InsertSorted(list1, int.Parse(listValues[0]));
                InsertSorted(list2, int.Parse(listValues[1]));
            }
        }
    }

    private static void InsertSorted(LinkedList<int> list, int value)
    {
        if (list.Count == 0)
        {
            list.AddFirst(value);
            return;
        }

        var listElement = list.First;
        for (int i = 0; i < list.Count; i++)
        {
            if (listElement!.Value > value)
            {
                list.AddBefore(listElement, value);
                return;
            }
            listElement = listElement.Next;
        }

        list.AddLast(value);
    }

    private static int CalculateListSimilarityScore(LinkedList<int> list1, LinkedList<int> list2)
    {
        int listSimilarityScore = 0;

        var list2Element = list2.First;
        var multiplierDict = new Dictionary<int, int>();

        for (int i = 0; i < list2.Count; i++)
        {
            if (multiplierDict.TryGetValue(list2Element!.Value, out var multiplier))
            {
                multiplierDict[list2Element.Value] = multiplier + 1;
            }
            else
            {
                multiplierDict.Add(list2Element.Value, 1);
            }

            list2Element = list2Element.Next;
        }

        var list1Element = list1.First;

        for (int i = 0; i < list1.Count; i++)
        {
            var list1Value = list1Element!.Value;

            if (multiplierDict.TryGetValue(list1Element!.Value, out var multiplier))
            {
                listSimilarityScore += list1Value * multiplier;
            }

            list1Element = list1Element.Next;
        }

        return listSimilarityScore;
    }
}
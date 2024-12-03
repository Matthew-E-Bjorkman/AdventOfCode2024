
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
        var listDistance = CalculateListDistance(list1, list2);

        Console.WriteLine("Total distance between the two lists: " + listDistance);
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

    private static int CalculateListDistance(LinkedList<int> list1, LinkedList<int> list2)
    {
        var totalListDistance = 0;
        var list1Node = list1.First;
        var list2Node = list2.First;

        for (int i = 0; i < list1.Count; i++)
        {
            var list1Value = list1Node!.Value;
            var list2Value = list2Node!.Value;

            totalListDistance += Math.Abs(list1Value - list2Value);

            list1Node = list1Node.Next;
            list2Node = list2Node.Next;
        }

        return totalListDistance;
    }
}
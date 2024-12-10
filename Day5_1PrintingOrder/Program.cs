using System.Collections;
using System.Data;

//  Read input into two objects, a hashtable for the rules, and a list for the updates
var inputObjects = PopulateListFromInput();

var pagingRules = inputObjects.Item1;
var printingOrders = inputObjects.Item2;

var sumOfCorrectMiddlePages = ValidatePagingOrders(pagingRules, printingOrders);

Console.WriteLine("Sum of middles pages in correct orders: " + sumOfCorrectMiddlePages);
Console.WriteLine("Press any key to exit.");
Console.ReadKey();

(Hashtable, List<List<int>>) PopulateListFromInput()
{
    var fileName = Path.Combine(Environment.CurrentDirectory, "Input.txt");

    if (!File.Exists(fileName))
    {
        Console.Error.WriteLine("Provided input file does not exist.");
        Console.ReadKey();
        Environment.Exit(1);
    }
    
    (Hashtable, List<List<int>>) inputObjects = (new Hashtable(), new List<List<int>>());

    using (var file = new StreamReader(fileName))
    {
        // Read the file line by line
        while (true)
        {
            var fileLine = file.ReadLine();

            // If the line is empty, we have reached the end of the rules, and the start of the printing orders
            if (string.IsNullOrEmpty(fileLine))
                break;

            // Split the line by | separator into the list of values
            var listValues = fileLine.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToList();

            // Check for the key in the hashtable
            var hashKeyExists = inputObjects.Item1.ContainsKey(listValues[1]);
            var hashList = hashKeyExists ? inputObjects.Item1[listValues[1]] as List<int> : new List<int>();

            // Append the rule to this page's list of rules
            hashList!.Add(listValues[0]);

            // Insert the rules into the hashtable (12|27 -> 12 must precede 27. 27 is the key, 12 is in the value list)
            if (!hashKeyExists) 
            {
                inputObjects.Item1.Add(listValues[1], hashList);
            } 
        }

        while (true)
        {
            var fileLine = file.ReadLine();

            // If the line is empty, we have reached the end of the printing orders
            if (string.IsNullOrEmpty(fileLine))
                break;

            // Split the line by | separator into the list of values
            var listValues = fileLine.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToList();

            // Insert the rules into the list of printing orders
            inputObjects.Item2.Add(listValues);
        }
    }

    return inputObjects;
}

int ValidatePagingOrders(Hashtable pagingRules, List<List<int>> printingOrders)
{
    int sumOfCorrectMiddlePages = 0;

    //  For each line in the input
    foreach (var order in printingOrders)
    {
        var isOrderValid = true;
        var illegalPageNumbers = new HashSet<int>();
        //  For each number in the line
        foreach (var page in order)
        {
            //  Check it against the current hashset of rules.
            if (illegalPageNumbers.Contains(page))
            {
                isOrderValid = false;
                break;
            }

            //  Add it's rules to a new hashset in reverse order. (e.g. if 12 must precede 27, then add 12 to the hashset when 27 is encountered).
            var pageRules = pagingRules[page] as List<int>;
            if (pageRules != null)
            {
                illegalPageNumbers.UnionWith(pageRules);
            }
        }

        //  If no failures are encountered, return the middle number in the list.
        if (isOrderValid)
        {
            sumOfCorrectMiddlePages += order[order.Count / 2];
        }
    }

    return sumOfCorrectMiddlePages;
}
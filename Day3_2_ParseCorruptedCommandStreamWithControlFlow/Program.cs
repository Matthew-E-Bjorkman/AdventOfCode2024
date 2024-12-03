
using System.Text.RegularExpressions;

var fileName = Path.Combine(Environment.CurrentDirectory, "Input.txt");

if (!File.Exists(fileName))
{
    Console.Error.WriteLine("Provided input file does not exist.");
    return;
}

var input = string.Empty;

using (var file = new StreamReader(fileName))
{
    input = file.ReadToEnd();
}

var validInstructions = Regex.Matches(input, "(mul\\([0-9]{1,3},[0-9]{1,3}\\))|(do\\(\\))|(don't\\(\\))");

var operationResult = 0;
var mulInstructionIsActive = true;

foreach (var instruction in validInstructions.ToList())
{
    if (instruction.Value.Contains("don't"))
    {
        mulInstructionIsActive = false;
        continue;
    }

    if (instruction.Value.Contains("do"))
    {
        mulInstructionIsActive = true;
        continue;
    }

    if (!mulInstructionIsActive)
        continue;

    var instructionValues = Regex.Matches(instruction.Value, "[0-9]{1,3}");
    operationResult += int.Parse(instructionValues[0].Value) * int.Parse(instructionValues[1].Value);
}

Console.WriteLine("Result of operation: " + operationResult);
Console.WriteLine("Press any key to exit.");
Console.ReadKey();
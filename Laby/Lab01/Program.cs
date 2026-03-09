List<int> ints = new List<int>();

Console.WriteLine("Please provide numbers. They should fit in an int. Once you're done write 'ok'");

var input = Console.ReadLine();
while (input != "ok")
{
    if (int.TryParse(input, out int parsedInt) == false)
        Console.WriteLine($"{input} is not a number");
    else
        ints.Add(parsedInt);
    input = Console.ReadLine();
}

Console.WriteLine("Entered numbers in LIFO order:");
for (int i = ints.Count - 1; i >= 0; i--)
{
    Console.WriteLine(ints[i]);
}

Console.WriteLine("Press any key to continue..");
_ =Console.ReadLine();



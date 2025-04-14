// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

List<int> usesList = new List<int>();

for (int i = 0; i < 100000; i++)
{
	int D = 12;
	int uses = 0;
	while (D>=4)
	{
		uses++;
		if (RNG.Range(1, (D + 1)) < 3)
		{
			D -= 2;
		}
	}
	
		usesList.Add(uses);

}

float result = usesList.Sum();

Console.WriteLine(result/100000);
usesList.Sort();
usesList.Reverse();
Console.WriteLine(usesList[0]);
Console.WriteLine(	 usesList[usesList.Count-1]);
Console.WriteLine();
foreach (int i in usesList.Take(25))
{
    Console.WriteLine(i);
}



// See https://aka.ms/new-console-template for more information
public static class RNG
{
    public static Random Random = new Random();
    public static int Range(int a, int b)
    {
        return Random.Next(a, b);
    }
}






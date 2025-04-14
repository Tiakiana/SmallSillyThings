public static class RNG
{
    public static Random random = new Random();
    public static int Range(int min, int max)
    {
        return random.Next(min, max);

    }

}




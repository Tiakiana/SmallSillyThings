
public static class RNG
{
    public static Random Rnd = new Random();
    public static int D6 => Die6();
    public static int Die6()
    {
        return Rnd.Next(1, 7);
    }
}

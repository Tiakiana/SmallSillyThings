namespace ArsMagicaLib
{
    public static class RNG
    {
        private static System.Random random = new System.Random();

        public static int Range(int a, int b)
        {
            return random.Next(a,b);
        }
    }


    



}

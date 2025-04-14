namespace ArsMagicaLib
{
    public static class WeaponFactory
    {
        public static Weapon SwordAndShield
        {
            get { return new Weapon(2, 2, 7, 4, 1, "Sword and Shield"); }
        }
        public static Weapon ShortSpear
        {
            get { return new Weapon(5, 1, 2, 3, -0.5f, "ShortSpear"); }
        }
        public static Weapon Mace
        {
            get { return new Weapon(3, 2,3,5,-0.5f ,"Mace"); }
        }
        public static Weapon Axe
        {
            get { return new Weapon(3,1,2,6, -0.5f, "Axe"); }
        }
        public static Weapon Shortsword
        {
            get { return new Weapon(4, 2, 3, 3, -0.5f, "Short sword"); }
        }
        public static Weapon Longsword
        {
            get { return new Weapon(3, 1, 4, 4, -0.5f, "Long sword"); }
        }

        //Shields

        public static Weapon RoundShield
        {
            get { return new Weapon(-1, 1, 3,0, -0.5f, "Round Shield"); }
        }
        public static Weapon Buckler
        {
            get { return new Weapon(-1, 2, 2, 0, -0.5f, "Buckler"); }
        }
        public static Weapon KiteShield
        {
            get { return new Weapon(-1, 1, 4, 0, -1.0f, "Kite Shield"); }
        }

        public static Weapon TowerShield
        {
            get { return new Weapon(-3, -1, 6, 0, -2.0f, "Tower Shield"); }
        }

        public static Weapon QuarterStaff
        {
            get { return new Weapon(6, 4, 6, 3, -1.0f, "Quarterstaff"); }
        }
        public static Weapon GreatSword
        {
            get { return new Weapon(6, 4, 4, 8, -1.0f, "Greatsword"); }
        }
    }






}

namespace ArsMagicaLib
{
    public static class ArmorFactory
    {

        public static Armor NoArmor { get { return new Armor(0, "Nothing", 0); } }
        public static Armor FullChainMail { get { return new Armor(12, "Full Chainmail", -6); } }
        public static Armor LeatherScale { get { return new Armor(4, "Hauberk Leather Scale", -2); } }
        public static Armor LoricaSegmentata { get { return new Armor(9, "Lorica Segmentata", -4.5f); } }
        public static Armor SteelScale{ get { return new Armor(10, "Steel Scale Mail", -5f); } }
        public static Armor Quilted{ get { return new Armor(1, "Quilted Fur", -.5f); } }

    }






}

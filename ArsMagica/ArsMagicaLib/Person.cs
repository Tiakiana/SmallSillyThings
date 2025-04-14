namespace ArsMagicaLib
{
    public class Person
    {
        public int Skill, Quickness, Stamina, Dexterity, Size,Strength;
        public int CarryOver;
        public int Wounds;
        public Weapon Weapon;
        public Armor Armor;


        public int Wins = 0;
        public int Battles = 0;



        public Person(int skill, int quickness, int stamina, int dexterity, int size, int carryOver, int strength, Armor armor, Weapon weapon)
        {
            Skill = skill;
            Quickness = quickness;
            Stamina = stamina;
            Dexterity = dexterity;
            Size = size;
            CarryOver = carryOver;
            Strength = strength;
            Weapon = weapon;
            Armor = armor;


        }
        public override string ToString()
        {
            return $"CO: {CarryOver} \nWO:{Wounds}";
        }
        public void Reset()
        {
            CarryOver = 0;
            Wounds = 0;
        }

        public int Inititiative
        {
            get
            {
                return Quickness + Skill + Weapon.Initiative;
            }
        }


        public int Attack
        {
            get
            {
                return Dexterity + Skill+Weapon.Attack + Encumberance;
            }
        }

        public int Defence
        {
            get
            {
                return Quickness + Skill + Weapon.Defence+Encumberance-Size;
            }
        }
        public int Damage
        {
            get
            {
                return Strength + Size + Weapon.Damage;
            }
        }
        public int Soak
        {
            get
            {
                return Stamina + Size+ Armor.Protection;
            }
        }

        public int Encumberance
        {
            get
            {
                int result = (int)System.Math.Ceiling(Armor.Load + Weapon.Load) + Strength;

                return result<=0?result:0;
            }
        }

    }






}

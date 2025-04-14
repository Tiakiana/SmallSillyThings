using System.Collections.Generic;
using System.Linq;

namespace ArsMagicaLib
{
    public class CombatManager
    {


        public List<Person> People = new List<Person>();

        List<Armor> Armors = new List<Armor>() { ArmorFactory.LeatherScale, ArmorFactory.LoricaSegmentata, ArmorFactory.FullChainMail, ArmorFactory.SteelScale , ArmorFactory.NoArmor,ArmorFactory.Quilted};
        List<Weapon> OneHandedWeapons = new List<Weapon>() { WeaponFactory.Axe, WeaponFactory.Longsword, WeaponFactory.Mace, WeaponFactory.ShortSpear, WeaponFactory.Shortsword };
        List<Weapon> Shields = new List<Weapon>() { WeaponFactory.Buckler, WeaponFactory.RoundShield, WeaponFactory.KiteShield, WeaponFactory.TowerShield };

        List<Weapon> TwoHandedWeapons = new List<Weapon>() { WeaponFactory.GreatSword, WeaponFactory.QuarterStaff };

        public Person a = new Person(3, 3, 3, 3, 3, 0, 5, ArmorFactory.NoArmor, WeaponFactory.ShortSpear);
        public Person b = new Person(3, 3, 3, 3, 3, 0, 5, ArmorFactory.NoArmor, WeaponFactory.SwordAndShield);




        public CombatManager()
        {

            List<Weapon> swordandshield = new List<Weapon>();
            for (int i = 0; i < OneHandedWeapons.Count; i++)
            {

                for (int shiel = 0; shiel < Shields.Count; shiel++)
                {

                    swordandshield.Add(OneHandedWeapons[i] + Shields[shiel]);
                }


            }
            OneHandedWeapons.AddRange(swordandshield);
            OneHandedWeapons.AddRange(TwoHandedWeapons);

            foreach (var armor in Armors)
            {
                foreach (var item in OneHandedWeapons)
                {
                    People.Add(new Person(1, 1, 1, 1, 1,0,1, armor, item));
                }
            }

            for (int i = 0; i < People.Count - 1; i++)
            {
                for (int x = i + 1; x < People.Count; x++)
                {
                    a = People[i];
                    b = People[x];
                    a.Reset();
                    b.Reset();
                    for (int ix = 0; ix < 100; ix++)
                    {
                        while (a.Wounds < 10 && b.Wounds < 10)
                        {
                            Combat2();
                        }
                        if (a.Wounds > b.Wounds)
                        {
                            b.Wins++;
                        }
                        else
                        {
                            a.Wins++;
                        }
                        b.Battles++;
                        a.Battles++;
                        a.Reset();
                        b.Reset();

                    }
                }
            }

            People = People.OrderByDescending(x => x.Wins).ToList();

            foreach (var item in People)
            {
                System.Console.WriteLine($"{(((float)item.Wins / (float)item.Battles) * 100).ToString("00.00")} % wins : {item.Weapon.Name} wearing {item.Armor.Name}");
            }


        }

        public int DefenceTotal(Person pers, int roll)
        {
            return roll + pers.Defence + pers.CarryOver;
        }
        public int AttackTotal(Person pers, int roll)
        {
            return roll + pers.Attack + pers.CarryOver;
        }

        public int DefenceTotal2(Person pers, int roll)
        {
            return roll + pers.Defence;//+ pers.CarryOver;
        }
        public int AttackTotal2(Person pers, int roll)
        {
            return roll + pers.Attack; //+ pers.CarryOver;
        }



        public void Combat()
        {
            int rollA = RollDie();
            int rollB = RollDie();

            if (rollA == -1)
            {
                a.CarryOver = 0;
                rollA = 0;
            }
            if (rollB == -1)
            {
                b.CarryOver = 0;
                rollB = 0;
            }

            int attackA = AttackTotal(a, rollA);
            int attackB = AttackTotal(b, rollB);

            int defenceA = DefenceTotal(a, rollA);
            int defenceB = DefenceTotal(b, rollB);


            if (attackA > defenceB)
            {

                if (a.Damage + attackA - defenceB - b.Soak <= 4)
                {
                    a.CarryOver += attackA - defenceB;
                }
                else if (RNG.Range(0, 2) == 1)
                {
                    a.CarryOver += attackA - defenceB;
                }
                else
                {
                    int damage = a.Damage + attackA - defenceB - b.Soak;

                    int wounds = 0;
                    for (int i = 1; i < damage; i++)
                    {
                        if (i % 5 == 0)
                        {
                            wounds++;
                        }
                    }
                    a.CarryOver = 0;
                    b.Wounds += wounds;
                }
            }

            if (attackB > defenceA)
            {
                // vil jeg noget her?
                if (b.Damage + attackB - defenceA - a.Soak <= 4)
                {
                    b.CarryOver += attackB - defenceA;
                }
                else if (RNG.Range(0, 2) == 1)
                {
                    b.CarryOver += attackB - defenceA;
                }
                else
                {
                    int damage = b.Damage + attackB - defenceA - a.Soak;

                    int wounds = 0;
                    for (int i = 1; i < damage; i++)
                    {
                        if (i % 5 == 0)
                        {
                            wounds++;
                        }
                    }

                    b.CarryOver = 0;
                    a.Wounds += wounds;
                }
            }
        }

        public void Combat2()
        {
            int rollA = RollDie();
            int rollB = RollDie();

            if (rollA == -1)
            {
                //   a.CarryOver = 0;
                rollA = 0;
            }
            if (rollB == -1)
            {
                //  b.CarryOver = 0;
                rollB = 0;
            }

            int attackA = AttackTotal2(a, rollA);
            int attackB = AttackTotal2(b, rollB);

            int defenceA = DefenceTotal2(a, rollA);
            int defenceB = DefenceTotal2(b, rollB);


            if (attackA > defenceB)
            {
                int damage = a.Damage + RollDie() - RollDie() - b.Soak;
                int wounds = 0;
                for (int i = 1; i < damage; i++)
                {
                    if (i % 5 == 0)
                    {
                        wounds++;
                    }
                }
              //  a.CarryOver = 0;
                b.Wounds += wounds;

            }

            if (attackB > defenceA)
            {
                // vil jeg noget her?
                if (b.Damage + attackB - defenceA - a.Soak <= 4)
                {
                    b.CarryOver += attackB - defenceA;
                }
                else if (RNG.Range(0, 2) == 1)
                {
                    b.CarryOver += attackB - defenceA;
                }
                else
                {
                    int damage = b.Damage + RollDie() - RollDie()- a.Soak;

                    int wounds = 0;
                    for (int i = 1; i < damage; i++)
                    {
                        if (i % 5 == 0)
                        {
                            wounds++;
                        }
                    }

                    b.CarryOver = 0;
                    a.Wounds += wounds;
                }
            }
        }



        public int RollDie()
        {
            int multiplier = 1;
        A:
            int roll = RNG.Range(1, 11);

            if (roll == 1)
            {
                multiplier *= 2;
                goto A;
            }
            else if (roll == 10)
            {
                if (multiplier == 1)
                {
                    return -1;
                }
            }
            return roll * multiplier;
        }

       


    }






}

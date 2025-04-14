using System;

namespace ArsMagicaLib
{

    public  class Weapon
    {
        public int Initiative, Attack, Defence, Damage;
        public float Load;
        public string Name;

        public Weapon(int initiative, int attack, int defence, int damage, float load, string name)
        {
            Initiative = initiative;
            Attack = attack;
            Defence = defence;
            Damage = damage;
            Load = load;
            Name = name;
        }

        public static Weapon operator +(Weapon a, Weapon b) => new Weapon(b.Initiative+a.Initiative,a.Attack+b.Attack,a.Defence+b.Defence,a.Damage+b.Damage,a.Load+b.Load,a.Name+" & " +b.Name);

    }


    



}

namespace MordheimFighter
{
    public class Combatant
    {

        public List<Ability> Abilities = new List<Ability>();

        public string Name { get; set; }
        public int Movement { get; set; }
        public int WeaponSkill { get; set; }
        public int BallisticSkill { get; set; }
        public int Strength { get; set; }
        public int Toughness { get; set; }
        public int Wounds { get; set; }
        public int Initiative { get; set; }
        public int Attacks { get; set; }
        public int Leadership { get; set; }
        public int Damage { get; set; }
        public int ArmorSave { get; set; }

        public FighterState FighterState;

        public Combatant(int movement = 4, int weaponSkill = 3, int ballisticSkill = 3, int strength = 3, int toughness = 3, int wounds = 1, int initiative = 3, int attacks = 1, int leadership = 7)
        {
            Movement = movement;
            WeaponSkill = weaponSkill;
            BallisticSkill = ballisticSkill;
            Strength = strength;
            Toughness = toughness;
            Wounds = wounds;
            Initiative = initiative;
            Attacks = attacks;
            Leadership = leadership;
        }

        public Combatant(string name,  int weaponSkill = 3, int ballisticSkill = 3, int strength = 3, int toughness = 3, int wounds = 1, int initiative = 3, int attacks = 1, int leadership = 7)
        {
            Name = name;
            Movement = 4;
            WeaponSkill = weaponSkill;
            BallisticSkill = ballisticSkill;
            Strength = strength;
            Toughness = toughness;
            Wounds = wounds;
            Initiative = initiative;
            Attacks = attacks;
            Leadership = leadership;
        }


        public Combatant(int weaponSkill = 3, int ballisticSkill = 3, int strength = 3, int toughness = 3, int wounds = 1, int initiative = 3, int attacks = 1, int leadership = 7)
        {
            Movement = 4;
            WeaponSkill = weaponSkill;
            BallisticSkill = ballisticSkill;
            Strength = strength;
            Toughness = toughness;
            Wounds = wounds;
            Initiative = initiative;
            Attacks = attacks;
            Leadership = leadership;
        }


    }
}

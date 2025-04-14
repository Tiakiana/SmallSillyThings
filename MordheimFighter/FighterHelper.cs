namespace MordheimFighter
{

    public static class FighterHelper
    {

        public static int GetToHitTarget(Combatant attacker, Combatant defender)
        {
            return attacker.WeaponSkill > defender.WeaponSkill ? 3 : defender.WeaponSkill > attacker.WeaponSkill * 2 ? 5 : 4;
        }


        public static int GetToWoundTarget(Combatant attacker, Combatant defender)
        {
            return Math.Clamp(4 + (defender.Toughness - attacker.Strength), 2, 7);
        }

        public static FighterState GetFighterState(Combatant fighter)
        {
            if (fighter.Wounds <= 0)
            {
                return FighterState.OutOfAction;
            }
            else if (fighter.Wounds == 1)
            {
                return FighterState.Down;
            }
            else if (fighter.Wounds == 2)
            {
                return FighterState.Stunned;
            }
            else
            {
                return FighterState.Normal;
            }
        }

    }

}
namespace MordheimFighter
{
    public struct Resultat
    {
        public Combatant Attacker, Defender;
        public bool Success;
        public int AttackerRoll;
        public int DefenderRoll;

        public Resultat(Combatant atk, Combatant defen, bool success, int roll)
        {
            Attacker = atk;
            Defender = defen;
            Success = success;
            AttackerRoll = roll;
        }
    }
}

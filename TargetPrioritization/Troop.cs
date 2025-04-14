namespace TargetPrioritization
{
    public class Troop
    {

        public static Dictionary<TroopType, List<TroopType>> CombatProfiler = new Dictionary<TroopType, List<TroopType>>();
        public int HP = 50;
        public Troop CrntTarget = null;
        public Engagement Engagement = null;
        public Leader General;

        public override string ToString()
        {
            return Type + $"({HP})[{General.Name}]";
        }
        public static void Init()
        {
            CombatProfiler.Add(TroopType.Archer, new List<TroopType> { TroopType.Spear, TroopType.Irregular, TroopType.Archer, TroopType.Shields, TroopType.Cavalry });
            CombatProfiler.Add(TroopType.Spear, new List<TroopType> { TroopType.Irregular, TroopType.Cavalry, TroopType.Spear, TroopType.Archer, TroopType.Shields, });
            CombatProfiler.Add(TroopType.Irregular, new List<TroopType> { TroopType.Cavalry, TroopType.Shields, TroopType.Irregular, TroopType.Archer, TroopType.Spear, });
            CombatProfiler.Add(TroopType.Cavalry, new List<TroopType> { TroopType.Archer, TroopType.Shields, TroopType.Cavalry, TroopType.Spear, TroopType.Irregular, });
            CombatProfiler.Add(TroopType.Shields, new List<TroopType> { TroopType.Spear, TroopType.Archer, TroopType.Shields, TroopType.Irregular, TroopType.Cavalry, });
        }
        public List<TroopType> PriorityTarget;
        public TroopType Type;
        public bool SpokenFor = false;
        public bool IsDead = false;
        public bool Available => !IsDead && !SpokenFor && Engagement == null;
        public Troop(TroopType type, Leader general)
        {
            Type = type;

            PriorityTarget = CombatProfiler[type];
            General = general;
        }

        public void TakeDamage(int dam)
        {
            HP -= dam;
            if (HP <= 0)
            {
                IsDead = true;
            }
        }


    }


}




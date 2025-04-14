using TargetPrioritization;

public class Engagement
{
    public List<Troop> SideA = new List<Troop>();
    public List<Troop> SideB = new List<Troop>();
    public bool MarkedForDeletion = false;
    public bool Resolved => SideA.Count == 0 || SideB.Count == 0;

    public void CombatStep()
    {
     //   Console.WriteLine("Combat betwixt: " + ToString());

        foreach (var item in SideA)
        {
            int die = 6 - Program.GetPriority(item, SideB[0]);
            int damage = 0;
            for (int dice = 0; dice < die; dice++)
            {
                damage += RNG.Range(1, 7);
            }
            //SideB[0].TakeDamage(damage);
            item.CrntTarget.TakeDamage(damage);
            Console.WriteLine("\t" + item.ToString() + " did " + damage + " damage to " + item.CrntTarget + $"({die} dice)");
        }

        foreach (var item in SideB)
        {
            int die = 6 - Program.GetPriority(item, SideA[0]);
            int damage = 0;
            for (int dice = 0; dice < die; dice++)
            {
                damage += RNG.Range(1, 7);
            }
            // SideA[0].TakeDamage(damage);
            item.CrntTarget.TakeDamage(damage);
            Console.WriteLine("\t" + item.Type.ToString() + " did " + damage + " damage to " + item.CrntTarget + $"({die} dice)");
        }

        for (int i = SideA.Count - 1; i >= 0; i--)
        {
            if (SideA[i].IsDead)
            {
                SideA.RemoveAt(i);
            }
        }
        for (int i = SideB.Count - 1; i >= 0; i--)
        {
            if (SideB[i].IsDead)
            {
                SideB.RemoveAt(i);
            }
        }


    }
    public override string ToString()
    {
        string res = "";
        foreach (var item in SideA)
        {
            res += item.Type.ToString() + $"[{item.General.Name}]" + " ";
        }
        res += " vs ";
        foreach (var item in SideB)
        {
            res += item.Type.ToString() +$"[{item.General.Name}]" +" ";
        }
        return res;
    }


}




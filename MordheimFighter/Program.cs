using System.IO.Pipes;

namespace MordheimFighter
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<Combatant> Attackers = new List<Combatant>() {

            new Combatant("No Modifiers",3, 3, 3, 3, 1, 1, 1, 1),
            new Combatant("+1 ws",4, 3, 3, 3, 1, 1, 1, 1),
            new Combatant("+1 s",3, 3,4, 3, 1, 1, 1, 1),
            new Combatant("+1 A",3, 3, 3, 3, 1, 1, 2, 1),

            };
            //Attackers[0].Abilities.Add(new RerollToHit());
            //Attackers[1].Abilities.Add(new RerollToHit());
            //Attackers[2].Abilities.Add(new RerollToHit());
            //Attackers[3].Abilities.Add(new RerollToHit());


            foreach (var item in Attackers)
            {


            Combatant attacker = new Combatant(3, 3, 3, 3, 1, 1, 1, 1);
                attacker = item;
            Combatant defender = new Combatant(3, 3, 3, 3, 1, 1, 1, 1);
                //defender.Abilities.Add(new StepAside());
                defender.Abilities.Add(new Parry());
            //   defender.ArmorSave = 4;

   //         defender.ArmorSave = 0;

            float success = 0;
            for (int i = 0; i < 10000; i++)
            {
            Resultat resultat = new Resultat(attacker, defender, false, RNG.D6);

                for (int atk = 0; atk < attacker.Attacks; atk++)
                {
                        var pipeline = new List<Func<Resultat, Result<Resultat>>> {
                        CombatPipeline.RollToHit
                        };

                        if (attacker.Abilities.Any(x=> x is RerollToHit))
                        {
                            pipeline[0] = (ctx) => CombatPipeline.RollToHit(ctx).OnFailure(CombatPipeline.RerollToHit);
                        }


                        if (defender.Abilities.Any(x => x is Parry))
                        {
                            pipeline.Add(CombatPipeline.Parry);
                        }

                        if (defender.Abilities.Any(x=> x is StepAside))
                        {
                            pipeline.Add(CombatPipeline.RollStepAside);
                        }

                        if (defender.ArmorSave>0)
                        {
                            pipeline.Add(CombatPipeline.RollArmorSave);
                        }

                        pipeline.Add(CombatPipeline.RollToWound);
                        pipeline.Add(CombatPipeline.RollArmorSave);

                        Result<Resultat> result = Result<Resultat>.Success(resultat);
                        foreach (var step in pipeline)
                        {
                            result = result.Bind(step);
                            if (!result.IsSuccess) break; 
                        }


                        //Hardcoded Example:
                        //    Resultat res = Result<Resultat>
                        //.Success(resultat)
                        //.Bind(CombatPipeline.RollToHit)

                        // // .OnFailure(CombatPipeline.RerollToHit)
                        ////  .Bind(CombatPipeline.Parry)
                        ////  .Bind(CombatPipeline.RollStepAside)

                        //.Bind(CombatPipeline.RollToWound)
                        //.Bind(CombatPipeline.RollArmorSave)
                        //.Value;
                        ////  );
                        if (result.IsSuccess)
                        {

                            success++;
                            continue;
                        }
                    }
                //       Console.WriteLine();
            }
            Console.WriteLine(attacker.Name +": "+ "Chance of landing 1 wound: " + (float)success / 100f);
            }

        }


    }

    public static class CombatPipeline
    {
        public static Result<Resultat> RollToHit(Resultat context)
        {
            context.AttackerRoll = RNG.D6;
         //     Console.WriteLine("attacker rolls " + context.AttackerRoll);
            int target = FighterHelper.GetToHitTarget(context.Attacker, context.Defender);
            bool hit = target <= context.AttackerRoll;
            context.Success = hit;
            return hit ? Result<Resultat>.Success(context) : Result<Resultat>.Failure(context);
        }

        public static Result<Resultat> Parry(Resultat context)
        {
            context.DefenderRoll = RNG.D6;
         //   Console.WriteLine("Defender rolls to Parry: " + context.DefenderRoll + " vs " + context.AttackerRoll);
            context.Success = context.DefenderRoll <= context.AttackerRoll;
            return context.Success ? Result<Resultat>.Success(context) : Result<Resultat>.Failure(context);

        }
        public static Result<Resultat> RerollToHit(Resultat context)
        {

            context.AttackerRoll = RNG.D6;
         //    Console.WriteLine("attacker Rerolls " + context.AttackerRoll);

            context.Success = FighterHelper.GetToHitTarget(context.Attacker, context.Defender) <= context.AttackerRoll;
            return context.Success ? Result<Resultat>.Success(context) : Result<Resultat>.Failure(context);

        }

        public static Result<Resultat> RollToWound(Resultat context)
        {
            context.AttackerRoll = RNG.D6;
            // Console.WriteLine("attacker rolls to wound " + context.AttackerRoll);
            context.Success = FighterHelper.GetToWoundTarget(context.Attacker, context.Defender) <= context.AttackerRoll;
            return context.Success ? Result<Resultat>.Success(context) : Result<Resultat>.Failure(context);
        }

        public static Result<Resultat> RollArmorSave(Resultat context)
        {
            if (context.Defender.ArmorSave == 0)
            {
                return Result<Resultat>.Success(context);
            }
            int armorModifier = Math.Clamp(context.Attacker.Strength - 3, 0, 7);
            
            int target = context.Defender.ArmorSave + armorModifier;
            if (target < 7 && target > 0)
            {

                context.DefenderRoll = RNG.D6;
                //   Console.WriteLine("Defender rolls for armor: " + context.DefenderRoll);
                context.Success = context.DefenderRoll < target;
            }
            return context.Success ? Result<Resultat>.Success(context) : Result<Resultat>.Failure(context);
        }

        public static Result<Resultat> RollStepAside(Resultat context)
        {
            context.DefenderRoll = RNG.D6;
            Console.WriteLine("Defender tried to step aside: " + context.DefenderRoll);
            context.Success = context.DefenderRoll < 5;
            return context.Success ? Result<Resultat>.Success(context) : Result<Resultat>.Failure(context);
        }


        //public static Result<Resultat> RollInjury(Resultat context)
        //{
        //    int roll = Random.Shared.Next(1, 7);
        //    context.InjuryResult = roll == 1 ? "Knocked Down" : (roll <= 3 ? "Stunned" : "Out of Action");
        //    return Result<Resultat>.Success(context);
        //}
    }
}

public class Ability
{

}

public class Parry: Ability
{

}

public class StepAside: Ability
{

}

public class RerollToHit : Ability
{

}

using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.ComponentModel;
namespace TargetPrioritization
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<Engagement> Engagements = new List<Engagement>();
            Troop.Init();

            Leader GeneralA = new Leader() { Name = "A" };
            Leader GeneralB = new Leader() { Name = "B" };
            GeneralA.Skill = 5;

            GeneralA.Troops.Add(new Troop(TroopType.Archer,GeneralA));
            GeneralA.Troops.Add(new Troop(TroopType.Spear, GeneralA));
            GeneralA.Troops.Add(new Troop(TroopType.Shields, GeneralA));
            GeneralA.Troops.Add(new Troop(TroopType.Cavalry, GeneralA));
            GeneralA.Troops.Add(new Troop(TroopType.Irregular, GeneralA));

            GeneralB.Troops.Add(new Troop(TroopType.Archer, GeneralB));
            GeneralB.Troops.Add(new Troop(TroopType.Spear, GeneralB));
            GeneralB.Troops.Add(new Troop(TroopType.Shields, GeneralB));

            GeneralB.Troops.Add(new Troop(TroopType.Cavalry, GeneralB));
            GeneralB.Troops.Add(new Troop(TroopType.Irregular, GeneralB));

            GeneralA.Roll = RNG.Range(1, 11);
            GeneralB.Roll = RNG.Range(1, 11);


            for (int rounds = 0; rounds < 10; rounds++)
            {
                Console.WriteLine();
                Console.WriteLine($"Round " + (rounds + 1));
                Console.WriteLine();
                GeneralA.Initiative = RNG.Range(1, 11);
                GeneralB.Initiative = RNG.Range(1, 11);

                if (!GeneralA.Troops.Any(x=> !x.IsDead) || !GeneralB.Troops.Any(x=> !x.IsDead))
                {
                    Console.WriteLine("Battle is concluded");

                    if (!GeneralA.Troops.Any(x=> !x.IsDead))
                    {
                        Console.WriteLine("General A won");
                    }
                    else
                    {
                        Console.WriteLine("General B won");

                    }

                    break;
                }


                //New round


                EngagementRound(Engagements, GeneralA, GeneralB);

                Console.WriteLine();

                Console.WriteLine("\t\t COMBAT RESOLUTION:");

                foreach (var item in Engagements)
                {
                    Console.WriteLine(item.ToString());
                    item.CombatStep();
                }
                Console.WriteLine();
                Console.WriteLine();



                foreach (var item in Engagements)
                {


                    if (item.Resolved)
                    {
                        Console.WriteLine(item.ToString() +" is resolved");
                        foreach (var combatant in item.SideA)
                        {
                            combatant.SpokenFor = false;
                            combatant.Engagement = null;

                        }
                        foreach (var combatant in item.SideB)
                        {
                            combatant.SpokenFor = false;
                            combatant.Engagement = null;

                        }

                        item.SideB.Clear();
                        item.SideA.Clear();
                        item.MarkedForDeletion = true;
                    }
                }
                Engagements.RemoveAll(x => x.MarkedForDeletion);

                Console.WriteLine("\t\t TROOPS THINK FOR THEMSELVES!");

                //Try to shift target in engagement:
                foreach (var item in GeneralA.Troops.Where(x=> !x.IsDead))
                {
                    if (item.Engagement!=null)
                    {
                        if (item.CrntTarget.IsDead || RNG.Range(1,10)<6)
                        {
                            Troop crnt = item.CrntTarget;
                            item.CrntTarget= GetPriorityQueue(item, item.Engagement.SideA.Contains(item) ? item.Engagement.SideB : item.Engagement.SideA).Peek();
                            if (crnt != item.CrntTarget)
                            {
                            Console.WriteLine(item.Type + " shifts attention to " + item.CrntTarget.Type + " from " + crnt.Type);
                            }

                        }
                    }
                }
                foreach (var item in GeneralB.Troops.Where(x => !x.IsDead))
                {
                    if (item.Engagement != null)
                    {
                        if (item.CrntTarget.IsDead || RNG.Range(1, 10) < 6)
                        {
                            Troop crnt = item.CrntTarget;
                            item.CrntTarget = GetPriorityQueue(item, item.Engagement.SideA.Contains(item) ? item.Engagement.SideB : item.Engagement.SideA).Peek();
                            if (crnt != item.CrntTarget)
                            {
                                Console.WriteLine(item.Type + " shifts attention to " + item.CrntTarget.Type + " from " + crnt.Type);
                            }
                        }
                    }
                }

                foreach (var item in GeneralA.Troops)
                {
                    item.SpokenFor = false;
                }
                foreach (var item in GeneralB.Troops)
                {
                    item.SpokenFor = false;
                }


            }


        }

        private static void EngagementRound(List<Engagement> Engagements, Leader GeneralA, Leader GeneralB)
        {
            //Initiative
            Console.WriteLine("\t\t INITIATIVE");
            Console.WriteLine();
            while (GeneralA.Initiative == GeneralB.Initiative)
            {
                GeneralA.Initiative = RNG.Range(1, 11);
                GeneralB.Initiative = RNG.Range(1, 11);
            }
            Console.WriteLine($"General A rolled {GeneralA.Initiative} for initiative\nGeneral B Rolled {GeneralB.Initiative} for initiative");
            Console.WriteLine();
            Leader firstToAct = GeneralA.Initiative > GeneralB.Initiative ? GeneralA : GeneralB;
            Leader secondToAct = GeneralA.Initiative < GeneralB.Initiative ? GeneralA : GeneralB;
            int counter = 0;
            Console.WriteLine("General " + GeneralA.Name + " has the following troops to choose from: (" + GeneralA.Troops.Where(y => !y.IsDead).Sum(x=> x.HP)+" force)" );
            foreach (var item in GeneralA.Troops.Where(x => !x.IsDead))
            {
                Console.WriteLine(item.ToString() + " " + (item.Available?" (Free)":""));
            }
            Console.WriteLine("General " + GeneralB.Name + " has the following troops to choose from: (" + GeneralB.Troops.Where(y=> !y.IsDead).Sum(x=> x.HP)+" force)");
            foreach (var item in GeneralB.Troops.Where(x => !x.IsDead))
            {
                Console.WriteLine(item.ToString() + " " + (item.Available?" (Free)":""));
            }

            Console.WriteLine();
            Console.WriteLine("\t\t ENGAGEMENT PHASE:");

            // First To Act:
            while (counter < 20 && (firstToAct.Troops.Any(x => x.Available) || secondToAct.Troops.Any(x => x.Available)))
            {
                TryToEngage(Engagements, firstToAct, secondToAct);
                Console.WriteLine();
                TryToEngage(Engagements, secondToAct, firstToAct);
                counter++;
            }


        }

        private static void TryToEngage(List<Engagement> Engagements, Leader engager, Leader preventer)
        {
            //Hvem skal jeg sætte til at angribe?

          

            Troop choice = GetAttacker(engager.Troops, preventer.Troops);
            if (choice == null)
            {
                Console.WriteLine(engager.Name + " has no units to command!");
                return;
            }

            if (choice != null)
            {
                //Hvem vil han helst nakke?
                Troop target = GetPriorityQueue(choice, preventer.Troops).Peek();
                if (target != null)
                {
                    Console.WriteLine($"General {engager.Name} chooses " + choice.Type.ToString() + " to engage " + target.Type.ToString());
                    choice.SpokenFor = true;

                    //Hvem vil modstanderen sætte til at intercept'e?
                    Troop interceptor = GetBestCounter(choice, preventer.Troops);
                   

                    if (interceptor != null)
                    {
                        interceptor.SpokenFor = true;
                        Console.WriteLine($"General {preventer.Name} chooses " + interceptor.Type.ToString() + " as Interceptor");
                        engager.Roll = RNG.Range(1, 11)+engager.Momentum + engager.Skill;
                        preventer.Roll = RNG.Range(1, 11) +preventer.Momentum+preventer.Skill;

                        if (engager.Roll > preventer.Roll)
                        {
                            preventer.Momentum++;
                            Console.WriteLine($"General {engager.Name} succeeds");

                            if (target.Engagement != null)
                            {
                                if (target.Engagement.SideA.Contains(target))
                                {
                                    target.Engagement.SideB.Add(choice);
                                }
                                else
                                {
                                    target.Engagement.SideA.Add(choice);
                                }
                                choice.Engagement = target.Engagement;
                                Console.WriteLine(choice.Engagement.ToString());
                                choice.CrntTarget = target;

                            }
                            else
                            {
                                Engagement enga = new Engagement() { SideA = new List<Troop> { choice }, SideB = new List<Troop> { target } };
                                choice.Engagement = enga;
                                target.Engagement = enga;

                                choice.CrntTarget = target;
                                target.CrntTarget = choice;
                                Engagements.Add(enga);
                                target.SpokenFor = true;
                                Console.WriteLine(enga.ToString());

                            }
                        }
                        if (preventer.Roll > engager.Roll)
                        {
                            Console.WriteLine($"General {preventer.Name} intercepts!");
                            engager.Momentum++;


                            Engagement enga = new Engagement() { SideA = new List<Troop> { choice }, SideB = new List<Troop> { interceptor } };
                            Engagements.Add(enga);
                            choice.Engagement = enga;
                            choice.CrntTarget = interceptor;
                            interceptor.CrntTarget = choice;
                            interceptor.Engagement = enga;
                            Console.WriteLine(enga.ToString());
                        }
                        if (preventer.Roll == engager.Roll)
                        {
                            Console.WriteLine("General "+ preventer.Name + "´s " + interceptor.Type + " kept General " + engager.Name + "´s" + choice.Type + " busy! no one was engaged");
                        }

                    }
                    else
                    {
                        Console.WriteLine($"General {engager.Name} is uninhibited and charges in!");


                        if (target.Engagement != null)
                        {
                            if (target.Engagement.SideA.Contains(target))
                            {
                                target.Engagement.SideB.Add(choice);
                            }
                            else
                            {
                                target.Engagement.SideA.Add(choice);
                            }
                            choice.Engagement = target.Engagement;
                            choice.CrntTarget = target;
                            Console.WriteLine(choice.Engagement.ToString());

                        }
                        else
                        {
                            Engagement enga = new Engagement() { SideA = new List<Troop> { choice }, SideB = new List<Troop> { target } };
                            choice.Engagement = enga;
                            target.Engagement = enga;
                            Engagements.Add(enga);
                            target.SpokenFor = true;
                            Console.WriteLine(choice.Engagement.ToString());
                            choice.CrntTarget = target;
                            target.CrntTarget = choice;

                        }
                    }

                }
            }
        }

        private static Troop GetBestCounter(Troop choice, List<Troop> enemies)
        {
            enemies = enemies.Where(x => x.Available).ToList();
            Troop interceptor = null;
            int bestpriority = 99999;


            foreach (var item in enemies)
            {
                int counterstrength = GetPriority(item, choice);
                if (counterstrength < bestpriority)
                {
                    bestpriority = counterstrength;
                    interceptor = item;
                }
            }
            return interceptor;

        }

        private static Troop GetAttacker(List<Troop> listA, List<Troop> listB)
        {
            int indeks = 0;
            Troop choice = null;
            int bestprioty = 9999;
            foreach (var item in listA)
            {

                if (item.Available)
                {
                    var ting = GetPriorityQueue(item, listB);
                    if (ting != null && ting.Count>0)
                    {
                        
                        int prio = GetPriority(item, ting.Peek());
                        if (prio < bestprioty)
                        {
                            bestprioty = prio;
                            choice = listA[indeks];
                        }
                    }
                }
                indeks++;
            }
            return choice;
        }

        public static PriorityQueue<Troop, int> GetPriorityQueue(Troop troop, List<Troop> enemies)
        {
            PriorityQueue<Troop, int> res = new PriorityQueue<Troop, int>();
            List<TroopType> priority = Troop.CombatProfiler[troop.Type];
            enemies = enemies.Where(x => !x.IsDead).ToList();

            foreach (var item in enemies)
            {
                res.Enqueue(item, priority.IndexOf(item.Type) - (!item.SpokenFor ? 10 : 0));
            }
            return res;


        }

        public static int GetPriority(Troop attacker, Troop defender)
        {
            List<TroopType> priority = Troop.CombatProfiler[attacker.Type];
            return priority.IndexOf(defender.Type);
        }
    }


    public enum TroopType
    {
        Spear,
        Irregular,
        Cavalry,
        Shields,
        Archer
    }


}




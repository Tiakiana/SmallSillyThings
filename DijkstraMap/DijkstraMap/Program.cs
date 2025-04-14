using System;
using FloodSpill;
using FloodSpill.NeighbourProcessors;
using FloodSpill.Queues;
using FloodSpill.Utilities;

namespace DijkstraMap
{
    class Program
    {
        static void Main(string[] args)
        {
            var markMatrix = new int[10, 10];
            Position center = new Position(5, 5);
            Func<Position, Position, int> distanceToCenterComparer = // favours positions closer to center
                (first, second) => Position.Distance(center, first).CompareTo(Position.Distance(center, second));
            var priorityQueue = new PriorityPositionQueue(distanceToCenterComparer);
            var floodParameters = new FloodParameters(priorityQueue, 1, 1);
            new FloodSpiller().SpillFlood(floodParameters, markMatrix);
            string representation = MarkMatrixVisualiser.Visualise(markMatrix);
            Console.WriteLine(representation);
        }




	}
}

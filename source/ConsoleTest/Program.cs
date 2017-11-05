using System;
using System.Linq;
using GraphAI;

namespace ConsoleTest
{
    class Program
    {
        static void Main( string[] args )
        {
            // initialize
            var hanoi = new GraphAI.Hanoi.Hanoi(numRods: 3);
            var search = new BreadthFirstSearch(hanoi, hanoi.CreateSeed());
            var goal = hanoi.CreateGoal();

            // search
            int numIterations = 0;
            Path pathFound = null;
            while( true )
            {
                if( !search.TryIterate(isGoal: state => state.Equals(goal)) )
                {
                    // nothing more to search
                    break;
                }

                ++numIterations;
                pathFound = search.PathToGoal;
                if( pathFound != null )
                {
                    // goal found, no need to keep searching
                    break;
                }
            }

            // report
            Console.WriteLine($"Num. iterations: {numIterations}");
            if( pathFound == null )
            {
                Console.WriteLine("Goal not found!");
            }
            else
            {
                Console.WriteLine("Steps to goal:");
                pathFound.Actions.ToList().ForEach(a => Console.WriteLine(a.ToString()));
            }

            // wait to exit
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

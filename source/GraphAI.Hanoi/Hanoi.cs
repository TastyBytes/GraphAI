using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphAI.Hanoi
{
    public class Hanoi : IProblemSpace
    {
        private readonly int rodCount;

        public Hanoi( int numRods )
        {
            if( numRods <= 0 )
                throw new ArgumentOutOfRangeException(nameof(numRods));

            this.rodCount = numRods;
        }

        private Rod CreateRodWithDisks( int numDisks )
        {
            var rod = new Rod();
            for( int i = numDisks; i > 0; --i )
                rod = rod.AddOnTop(new Disk(i));
            return rod;
        }

        public GameState CreateSeed()
        {
            var array = Enumerable.Range(0, this.rodCount).Select(_ => new Rod()).ToArray();
            array[0] = this.CreateRodWithDisks(this.rodCount);
            return new GameState(array);
        }

        public GameState CreateGoal()
        {
            var array = Enumerable.Range(0, this.rodCount).Select(_ => new Rod()).ToArray();
            array[array.Length - 1] = this.CreateRodWithDisks(this.rodCount);
            return new GameState(array);
        }

        public MoveAction[] Extend( GameState state )
        {
            if( state == null )
                throw new ArgumentNullException();

            var result = new List<MoveAction>();
            for( int src = 0; src < state.Rods.Count; ++src )
            {
                // no disk
                if( state.Rods[src].IsEmpty )
                    continue;

                for( int dst = 0; dst < state.Rods.Count; ++dst )
                {
                    // no change
                    if( src == dst )
                        continue;

                    // disk too large
                    if( !state.Rods[dst].CanAdd(state.Rods[src].Top) )
                        continue;

                    // action possible
                    result.Add(new MoveAction(state, src, dst));
                }
            }
            return result.ToArray();
        }

        IAction[] IProblemSpace.Extend( IState state ) => this.Extend((GameState)state);
    }
}

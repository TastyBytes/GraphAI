using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GraphAI.Hanoi
{
    public class GameState : IState
    {
        public GameState( params Rod[] rods )
        {
            if( (rods?.Length ?? 0) == 0 )
                throw new ArgumentException("No rods!");

            if( rods.Where(r => r == null).Count() != 0 )
                throw new ArgumentException("null rod found!");

            if( rods.FirstOrDefault(r => !r.IsEmpty) == null )
                throw new ArgumentException("No disks!");

            this.Rods = new ReadOnlyCollection<Rod>((Rod[])rods.Clone());
        }

        public ReadOnlyCollection<Rod> Rods { get; }

        public bool Equals( GameState other )
        {
            if( other == null )
                return false;

            if( this.Rods.Count != other.Rods.Count )
                return false;

            for( int i = 0; i < this.Rods.Count; ++i )
            {
                if( !this.Rods[i].Equals(other.Rods[i]) )
                    return false;
            }

            return true;
        }

        public bool Equals( IState other )
        {
            if( other is GameState state )
                return this.Equals(state);
            else
                return false;
        }
    }
}

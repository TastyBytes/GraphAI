using System;
using System.Linq;

namespace GraphAI.Hanoi
{
    public class MoveAction : IAction
    {
        public MoveAction( GameState parent, int sourceRod, int destinationRod )
        {
            if( parent == null )
                throw new ArgumentNullException();

            if( sourceRod < 0 || parent.Rods.Count <= sourceRod )
                throw new ArgumentOutOfRangeException(nameof(sourceRod));

            if( destinationRod < 0 || parent.Rods.Count <= destinationRod )
                throw new ArgumentOutOfRangeException(nameof(destinationRod));

            this.OriginalState = parent;
            this.SourceRod = sourceRod;
            this.DestinationRod = destinationRod;
        }

        public GameState OriginalState { get; }
        public int SourceRod { get; }
        public int DestinationRod { get; }

        public GameState GetResult()
        {
            var rods = new Rod[this.OriginalState.Rods.Count];
            for( int i = 0; i < rods.Length; ++i )
            {
                var parentRod = this.OriginalState.Rods[i];
                if( i == this.SourceRod )
                {
                    rods[i] = parentRod.RemoveFromTop();
                }
                else if( i == this.DestinationRod )
                {
                    rods[i] = parentRod.AddOnTop(this.OriginalState.Rods[this.SourceRod].Top);
                }
                else
                {
                    rods[i] = new Rod(parentRod.Disks.ToArray());
                }
            }
            return new GameState(rods);
        }

        public override string ToString()
        {
            return $"{this.OriginalState.Rods[this.SourceRod].Top}: {this.SourceRod} --> {this.DestinationRod}";
        }

        IState IAction.GetResult() => this.GetResult();
    }
}

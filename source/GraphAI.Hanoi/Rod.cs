using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GraphAI.Hanoi
{
    public class Rod
    {
        public Rod( params Disk[] disks )
        {
            if( (disks?.Length ?? 0) == 0 )
                disks = new Disk[0];

            for( int i = 1; i < disks.Length; ++i )
            {
                if( disks[i - 1].Size <= disks[i].Size )
                    throw new ArgumentException();
            }

            this.Disks = new ReadOnlyCollection<Disk>((Disk[])disks.Clone());
        }

        public ReadOnlyCollection<Disk> Disks { get; }
        public bool IsEmpty => this.Disks.Count == 0;
        public Disk Top => this.Disks[this.Disks.Count - 1];

        public Rod RemoveFromTop()
        {
            if( this.IsEmpty )
                throw new InvalidOperationException();

            return new Rod(this.Disks.Take(this.Disks.Count - 1).ToArray());
        }

        public Rod AddOnTop( Disk disk )
        {
            return new Rod(this.Disks.Concat(new[] { disk }).ToArray());
        }

        public bool CanAdd( Disk disk )
        {
            return this.IsEmpty
                || this.Top.Size > disk.Size;
        }

        public bool Equals( Rod other )
        {
            if( other == null )
                return false;

            if( this.Disks.Count != other.Disks.Count )
                return false;

            for( int i = 0; i < this.Disks.Count; ++i )
            {
                if( !this.Disks[i].Equals(other.Disks[i]) )
                    return false;
            }

            return true;
        }
    }
}

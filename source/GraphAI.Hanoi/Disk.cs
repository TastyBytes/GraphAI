using System;

namespace GraphAI.Hanoi
{
    public struct Disk
    {
        public Disk( int size )
        {
            if( size <= 0 )
                throw new ArgumentOutOfRangeException(nameof(size));

            this.Size = size;
        }

        public int Size { get; }

        public bool Equals( Disk other )
        {
            return this.Size == other.Size;
        }
    }
}

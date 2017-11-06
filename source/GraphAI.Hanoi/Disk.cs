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

        public override string ToString()
        {
            return this.Size.ToString();
        }

        public bool Equals( Disk other )
        {
            return this.Size == other.Size;
        }
    }
}

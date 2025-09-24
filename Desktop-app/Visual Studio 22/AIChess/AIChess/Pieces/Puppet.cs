using System;
using System.Collections.Generic;

namespace AIChess.Pieces
{
    public abstract class Puppet
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsWhite { get; set; }
        public bool HasMoved { get; set; } = false;

        
        public abstract int Value { get; }
        public abstract string Symbol { get; }

        
        public abstract List<(int x, int y)> GetMoves(Core.Table table);

        
        public virtual Puppet Clone()
        {
            var clone = (Puppet)Activator.CreateInstance(this.GetType());
            clone.X = this.X;
            clone.Y = this.Y;
            clone.IsWhite = this.IsWhite;
            clone.HasMoved = this.HasMoved;
            return clone;
        }
    }
}

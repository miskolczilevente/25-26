using System;

namespace Wild_West
{
    public abstract class TownComponent
    {
        public int X { get; set; }
        public int Y { get; set; }
        public virtual bool Walkable => true;
    }
}

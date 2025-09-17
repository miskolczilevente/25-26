using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChess
{

    public abstract class Puppet
    {
        public abstract bool IsValidMove(int targetX, int targetY, Table table);
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsWhite { get; set; }   // új: bábu színe

        public abstract string Symbol { get; }
    }

}
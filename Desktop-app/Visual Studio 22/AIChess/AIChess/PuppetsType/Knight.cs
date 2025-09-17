using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChess.PuppetsType
{
    public class Knight : Puppet
    {
        public override string Symbol => "♘";

        public override bool IsValidMove(int targetX, int targetY, Table table)
        {
            int dx = Math.Abs(targetX - X);
            int dy = Math.Abs(targetY - Y);

            if ((dx == 1 && dy == 2) || (dx == 2 && dy == 1))
                return !table.IsOccupiedByAlly(targetX, targetY, IsWhite);

            return false;
        }
    }
}

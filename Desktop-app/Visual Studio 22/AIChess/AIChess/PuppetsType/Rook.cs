using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChess.PuppetsType
{
    public class Rook : Puppet
    {
        public override string Symbol => "♖";

        public override bool IsValidMove(int targetX, int targetY, Table table)
        {
            if (X != targetX && Y != targetY)
                return false;

            if (!table.IsPathClear(X, Y, targetX, targetY))
                return false;

            return !table.IsOccupiedByAlly(targetX, targetY, IsWhite);
        }
    }


}

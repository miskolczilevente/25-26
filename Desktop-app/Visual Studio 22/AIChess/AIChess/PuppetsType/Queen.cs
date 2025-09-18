using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChess.PuppetsType
{
    public class Queen : Puppet
    {
        public override string Symbol => "♕";

        public override bool IsValidMove(int targetX, int targetY, Table table)
        {
            int dx = Math.Abs(targetX - X);
            int dy = Math.Abs(targetY - Y);

            // Vezér: bármelyik irányban (vízszintes, függőleges, átlós)
            bool validDirection = (dx == dy) || (X == targetX) || (Y == targetY);
            if (!validDirection) return false;

            // Útvonal ellenőrzése
            if (!table.IsPathClear(X, Y, targetX, targetY)) return false;

            // Cél mező nem lehet saját bábu
            return !table.IsOccupiedByOwn(targetX, targetY, IsWhite);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChess.PuppetsType
{
    public class Bishop : Puppet
    {
        public override string Symbol => "♗";

        public override bool IsValidMove(int targetX, int targetY, Table table)
        {
            int dx = Math.Abs(targetX - X);
            int dy = Math.Abs(targetY - Y);

            // Csak átlós mozgás
            if (dx != dy) return false;

            // Útvonal ellenőrzése
            if (!table.IsPathClear(X, Y, targetX, targetY)) return false;

            // Cél mező nem lehet saját bábu
            return !table.IsOccupiedByAlly(targetX, targetY, IsWhite);
        }
    }

}

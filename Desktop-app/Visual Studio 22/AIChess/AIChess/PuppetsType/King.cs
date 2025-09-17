using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChess.PuppetsType
{
    public class King : Puppet
    {
        public override string Symbol => "♔";

        public override bool IsValidMove(int targetX, int targetY, Table table)
        {
            int dx = Math.Abs(targetX - X);
            int dy = Math.Abs(targetY - Y);

            // Király: csak egy mező minden irányban
            if (dx <= 1 && dy <= 1)
            {
                // Cél mező nem lehet saját bábu
                return !table.IsOccupiedByAlly(targetX, targetY, IsWhite);
            }

            // Sáncolás nincs implementálva (egyszerűsített változat)
            return false;
        }
    }

}

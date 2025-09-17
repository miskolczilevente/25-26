using System;

namespace AIChess
{
    public class Pawn : Puppet
    {
        public override string Symbol => "♙";

        public override bool IsValidMove(int targetX, int targetY, Table table)
        {
            int direction = IsWhite ? -1 : 1;
            int startRow = IsWhite ? 6 : 1;

            // sima lépés előre
            if (X == targetX && Y + direction == targetY && !table.IsOccupied(targetX, targetY))
                return true;

            // dupla lépés az első sorból
            if (X == targetX && Y == startRow && Y + 2 * direction == targetY &&
                !table.IsOccupied(targetX, Y + direction) &&
                !table.IsOccupied(targetX, targetY))
                return true;

            // ütés átlósan
            if (Math.Abs(targetX - X) == 1 && targetY == Y + direction &&
                table.IsOccupiedByOpponent(targetX, targetY, IsWhite))
                return true;

            return false;
        }
    }
}

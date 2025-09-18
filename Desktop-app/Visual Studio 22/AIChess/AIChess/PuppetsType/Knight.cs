namespace AIChess.PuppetsType
{
    public class Knight : Puppet
    {
        public override string Symbol => "♘";

        public override bool IsValidMove(int targetX, int targetY, Table table)
        {
            int dx = System.Math.Abs(targetX - X);
            int dy = System.Math.Abs(targetY - Y);

            // L alak (2+1 vagy 1+2)
            if ((dx == 2 && dy == 1) || (dx == 1 && dy == 2))
            {
                // nem léphet saját bábu helyére
                if (!table.IsOccupiedByOwn(targetX, targetY, IsWhite))
                    return true;
            }

            return false;
        }
    }
}

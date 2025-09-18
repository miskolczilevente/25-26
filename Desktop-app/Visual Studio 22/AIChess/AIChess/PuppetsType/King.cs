using AIChess;

public class King : Puppet
{
    public override string Symbol => "♔";

    public override bool IsValidMove(int targetX, int targetY, Table table)
    {
        int dx = Math.Abs(targetX - X);
        int dy = Math.Abs(targetY - Y);

        // Normál 1 mezős lépés minden irányban
        if (dx <= 1 && dy <= 1)
        {
            return !table.IsOccupiedByOwn(targetX, targetY, IsWhite);
        }

        // Sáncolás: 2 mező vízszintes, ugyanazon soron
        if (!HasMoved && dy == 0 && dx == 2)
        {
            int rookX = targetX > X ? 7 : 0;
            var rook = table.Pieces
                .OfType<Rook>()
                .FirstOrDefault(r => r.X == rookX && r.Y == Y && r.IsWhite == IsWhite);

            if (rook != null && !rook.HasMoved)
            {
                // Ellenőrizd, hogy az útvonal üres
                int step = rookX > X ? 1 : -1;
                for (int i = X + step; i != rookX; i += step)
                {
                    if (table.IsOccupied(i, Y)) return false;
                }
                return true;
            }
        }

        return false;
    }
}

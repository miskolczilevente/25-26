using AIChess;

public class Pawn : Puppet
{
    public override string Symbol => "♙";
    public bool HasMoved { get; set; } = false;

    public override bool IsValidMove(int targetX, int targetY, Table table)
    {
        int dir = IsWhite ? -1 : 1;
        int startRow = IsWhite ? 6 : 1;

        if (X == targetX && Y + dir == targetY && !table.IsOccupied(targetX, targetY))
            return true;

        if (!HasMoved && X == targetX && Y == startRow && Y + 2 * dir == targetY &&
            !table.IsOccupied(targetX, Y + dir) && !table.IsOccupied(targetX, targetY))
            return true;

        if (Math.Abs(targetX - X) == 1 && targetY == Y + dir &&
            table.IsOccupiedByOpponent(targetX, targetY, IsWhite))
            return true;

        return false;
    }

    public void Move(int targetX, int targetY)
    {
        X = targetX;
        Y = targetY;
        HasMoved = true;
    }
}

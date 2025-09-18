using AIChess;

public class Rook : Puppet
{
    public bool HasMoved { get; set; } = false;
    public override string Symbol => "♖";

    public override bool IsValidMove(int targetX, int targetY, Table table)
    {
        // Rook szabály
        return false;
    }
}
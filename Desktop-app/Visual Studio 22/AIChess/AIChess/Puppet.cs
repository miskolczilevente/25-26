using AIChess;

public abstract class Puppet
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsWhite { get; set; }
    public bool HasMoved { get; set; } = false; // hozzáadva minden bábuhoz

    public abstract string Symbol { get; }
    public abstract bool IsValidMove(int targetX, int targetY, Table table);
}

public class Field
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool CanMoveHere { get; set; } = false;

    public Field(int x, int y)
    {
        X = x;
        Y = y;
    }
}

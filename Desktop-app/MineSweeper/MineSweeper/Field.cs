namespace Minesweeper
{
    abstract class Field
    {
        public bool IsRevealed { get; protected set; }
        public bool IsFlagged { get; set; }

        public abstract void Reveal();
        public abstract char GetSymbol();
    }
}

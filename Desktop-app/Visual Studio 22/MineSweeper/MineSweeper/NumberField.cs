namespace Minesweeper
{
    class NumberField : Field
    {
        public int AdjacentBombs { get; private set; }

        public NumberField(int bombs)
        {
            AdjacentBombs = bombs;
        }

        public override void Reveal()
        {
            IsRevealed = true;
        }

        public override char GetSymbol()
        {
            if (IsFlagged) return '⚑';
            if (!IsRevealed) return '■';
            return char.Parse(AdjacentBombs.ToString());
        }
    }
}

namespace Minesweeper
{
    class BombField : Field
    {
        public override void Reveal()
        {
            IsRevealed = true;
        }

        public override char GetSymbol()
        {
            if (IsFlagged) return '⚑';
            if (!IsRevealed) return '■';
            return 'B';
        }
    }
}

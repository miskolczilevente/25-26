using AIChess.PuppetsType;
using System.Linq;
using System.Collections.Generic;

namespace AIChess
{
    public class GameController
    {
        private Table table;
        public bool IsWhiteTurn { get; private set; } = true;

        public GameController(Table table)
        {
            this.table = table;
        }

        public List<Field> GetMoveFields(Puppet piece)
        {
            var fields = new List<Field>();
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    if (piece.IsValidMove(x, y, table))
                        fields.Add(new Field(x, y) { CanMoveHere = true });

            // Sáncolás King esetén
            if (piece is King king && !king.HasMoved)
            {
                // Királyoldali
                var rookK = table.Pieces.FirstOrDefault(p => p is Rook r && r.IsWhite == king.IsWhite && !((Rook)p).HasMoved && r.X == 7 && r.Y == king.Y);
                if (rookK != null && !table.IsOccupied(5, king.Y) && !table.IsOccupied(6, king.Y))
                    fields.Add(new Field(6, king.Y) { CanMoveHere = true });

                // Vezéroldali
                var rookQ = table.Pieces.FirstOrDefault(p => p is Rook r && r.IsWhite == king.IsWhite && !((Rook)p).HasMoved && r.X == 0 && r.Y == king.Y);
                if (rookQ != null && !table.IsOccupied(1, king.Y) && !table.IsOccupied(2, king.Y) && !table.IsOccupied(3, king.Y))
                    fields.Add(new Field(2, king.Y) { CanMoveHere = true });
            }

            return fields;
        }

        public bool TryMove(Puppet piece, int x, int y, out string winner)
        {
            winner = null;
            if (piece == null || piece.IsWhite != IsWhiteTurn || !piece.IsValidMove(x, y, table))
                return false;

            var target = table.Pieces.FirstOrDefault(p => p.X == x && p.Y == y);
            if (target != null)
            {
                if (target is King)
                {
                    winner = IsWhiteTurn ? "Fehér" : "Fekete";
                    return true;
                }
                table.Pieces.Remove(target);
            }

            // Sáncolás
            if (piece is King king && Math.Abs(x - king.X) == 2)
            {
                int rookX = x > king.X ? 7 : 0;
                var rook = table.Pieces.OfType<Rook>().First(r => r.X == rookX && r.Y == king.Y && r.IsWhite == king.IsWhite);
                rook.X = x > king.X ? 5 : 3;
                rook.HasMoved = true;
            }

            piece.X = x;
            piece.Y = y;
            piece.HasMoved = true;

            IsWhiteTurn = !IsWhiteTurn;
            return true;
        }
    }
}

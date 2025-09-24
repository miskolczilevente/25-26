using System.Collections.Generic;

namespace AIChess.Pieces
{
    public class Pawn : Puppet
    {
        public override int Value => 1;
        public override string Symbol => IsWhite ? "♙" : "♟";

        public override List<(int x, int y)> GetMoves(Core.Table table)
        {
            var moves = new List<(int x, int y)>();
            int dir = IsWhite ? -1 : 1;
            int ny = Y + dir;

            
            bool InBoard(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;

            if (InBoard(X, ny) && !table.IsOccupied(X, ny))
                moves.Add((X, ny));

            
            if (!HasMoved && InBoard(X, Y + 2 * dir) && !table.IsOccupied(X, ny) && !table.IsOccupied(X, Y + 2 * dir))
                moves.Add((X, Y + 2 * dir));

            
            if (InBoard(X - 1, ny) && table.IsOccupiedByOpponent(X - 1, ny, IsWhite))
                moves.Add((X - 1, ny));
            if (InBoard(X + 1, ny) && table.IsOccupiedByOpponent(X + 1, ny, IsWhite))
                moves.Add((X + 1, ny));

            return moves;
        }

        public override Puppet Clone()
        {
            return new Pawn { X = this.X, Y = this.Y, IsWhite = this.IsWhite, HasMoved = this.HasMoved };
        }
    }
}

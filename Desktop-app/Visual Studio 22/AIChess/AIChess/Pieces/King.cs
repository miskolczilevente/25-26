using System.Collections.Generic;
using System.Linq;

namespace AIChess.Pieces
{
    public class King : Puppet
    {
        public override int Value => 1000;
        public override string Symbol => IsWhite ? "♔" : "♚";

        public override List<(int x, int y)> GetMoves(Core.Table table)
        {
            var moves = new List<(int x, int y)>();

            for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    int nx = X + dx, ny = Y + dy;
                    if (nx < 0 || nx >= 8 || ny < 0 || ny >= 8) continue;
                    if (!table.IsOccupiedByOwn(nx, ny, IsWhite))
                        moves.Add((nx, ny));
                }

           
            if (!HasMoved)
            {
                
                var rookK = table.Pieces.OfType<Rook>().FirstOrDefault(r => r.IsWhite == this.IsWhite && r.X == 7 && r.Y == this.Y && !r.HasMoved);
                if (rookK != null && table.IsPathClear(this.X, this.Y, 7, this.Y))
                {
                    
                    if (!table.IsOccupied(5, Y) && !table.IsOccupied(6, Y))
                        moves.Add((6, Y));
                }

                
                var rookQ = table.Pieces.OfType<Rook>().FirstOrDefault(r => r.IsWhite == this.IsWhite && r.X == 0 && r.Y == this.Y && !r.HasMoved);
                if (rookQ != null && table.IsPathClear(this.X, this.Y, 0, this.Y))
                {
                    if (!table.IsOccupied(1, Y) && !table.IsOccupied(2, Y) && !table.IsOccupied(3, Y))
                        moves.Add((2, Y));
                }
            }

            return moves;
        }

        public override Puppet Clone()
        {
            return new King { X = this.X, Y = this.Y, IsWhite = this.IsWhite, HasMoved = this.HasMoved };
        }
    }
}

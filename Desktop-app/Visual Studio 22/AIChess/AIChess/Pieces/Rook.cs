using System.Collections.Generic;

namespace AIChess.Pieces
{
    public class Rook : Puppet
    {
        public override int Value => 5;
        public override string Symbol => IsWhite ? "♖" : "♜";

        public override List<(int x, int y)> GetMoves(Core.Table table)
        {
            var moves = new List<(int x, int y)>();

            void AddDir(int dx, int dy)
            {
                int nx = X + dx, ny = Y + dy;
                while (nx >= 0 && nx < 8 && ny >= 0 && ny < 8)
                {
                    if (table.IsOccupiedByOwn(nx, ny, IsWhite)) break;
                    moves.Add((nx, ny));
                    if (table.IsOccupiedByOpponent(nx, ny, IsWhite)) break;
                    nx += dx; ny += dy;
                }
            }

            AddDir(1, 0); AddDir(-1, 0); AddDir(0, 1); AddDir(0, -1);
            return moves;
        }

        public override Puppet Clone()
        {
            return new Rook { X = this.X, Y = this.Y, IsWhite = this.IsWhite, HasMoved = this.HasMoved };
        }
    }
}

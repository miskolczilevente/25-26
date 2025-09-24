using System.Collections.Generic;

namespace AIChess.Pieces
{
    public class Knight : Puppet
    {
        public override int Value => 3;
        public override string Symbol => IsWhite ? "♘" : "♞";

        public override List<(int x, int y)> GetMoves(Core.Table table)
        {
            var moves = new List<(int x, int y)>();
            int[] dx = { -2, -1, 1, 2 };
            int[] dy = { -2, -1, 1, 2 };

            for (int i = 0; i < dx.Length; i++)
                for (int j = 0; j < dy.Length; j++)
                {
                    if (System.Math.Abs(dx[i]) == System.Math.Abs(dy[j])) continue;
                    int nx = X + dx[i], ny = Y + dy[j];
                    if (nx < 0 || nx >= 8 || ny < 0 || ny >= 8) continue;
                    if (!table.IsOccupiedByOwn(nx, ny, IsWhite))
                        moves.Add((nx, ny));
                }
            return moves;
        }

        public override Puppet Clone()
        {
            return new Knight { X = this.X, Y = this.Y, IsWhite = this.IsWhite, HasMoved = this.HasMoved };
        }
    }
}

using System.Collections.Generic;

namespace AIChess.Pieces
{
    public class Queen : Puppet
    {
        public override int Value => 9;
        public override string Symbol => IsWhite ? "♕" : "♛";

        public override List<(int x, int y)> GetMoves(Core.Table table)
        {
            var moves = new List<(int x, int y)>();

            
            var r = new Rook { X = this.X, Y = this.Y, IsWhite = this.IsWhite };
            moves.AddRange(r.GetMoves(table));
            var b = new Bishop { X = this.X, Y = this.Y, IsWhite = this.IsWhite };
            moves.AddRange(b.GetMoves(table));
            return moves;
        }

        public override Puppet Clone()
        {
            return new Queen { X = this.X, Y = this.Y, IsWhite = this.IsWhite, HasMoved = this.HasMoved };
        }
    }
}

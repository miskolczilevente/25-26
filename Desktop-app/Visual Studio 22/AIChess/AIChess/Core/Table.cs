using AIChess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIChess.Core
{
    public class Table
    {
        public List<Puppet> Pieces { get; set; } = new List<Puppet>();

        public Table(bool setup = true)
        {
            if (setup) InitializeStartingPosition();
        }

        private void InitializeStartingPosition()
        {
            Pieces.Clear();
            // White (bottom)
            Pieces.Add(new Rook { X = 0, Y = 7, IsWhite = true });
            Pieces.Add(new Knight { X = 1, Y = 7, IsWhite = true });
            Pieces.Add(new Bishop { X = 2, Y = 7, IsWhite = true });
            Pieces.Add(new Queen { X = 3, Y = 7, IsWhite = true });
            Pieces.Add(new King { X = 4, Y = 7, IsWhite = true });
            Pieces.Add(new Bishop { X = 5, Y = 7, IsWhite = true });
            Pieces.Add(new Knight { X = 6, Y = 7, IsWhite = true });
            Pieces.Add(new Rook { X = 7, Y = 7, IsWhite = true });
            for (int i = 0; i < 8; i++) Pieces.Add(new Pawn { X = i, Y = 6, IsWhite = true });

            // Black (top)
            Pieces.Add(new Rook { X = 0, Y = 0, IsWhite = false });
            Pieces.Add(new Knight { X = 1, Y = 0, IsWhite = false });
            Pieces.Add(new Bishop { X = 2, Y = 0, IsWhite = false });
            Pieces.Add(new Queen { X = 3, Y = 0, IsWhite = false });
            Pieces.Add(new King { X = 4, Y = 0, IsWhite = false });
            Pieces.Add(new Bishop { X = 5, Y = 0, IsWhite = false });
            Pieces.Add(new Knight { X = 6, Y = 0, IsWhite = false });
            Pieces.Add(new Rook { X = 7, Y = 0, IsWhite = false });
            for (int i = 0; i < 8; i++) Pieces.Add(new Pawn { X = i, Y = 1, IsWhite = false });
        }

        public bool IsOccupied(int x, int y) => Pieces.Any(p => p.X == x && p.Y == y);
        public bool IsOccupiedByOwn(int x, int y, bool isWhite) => Pieces.Any(p => p.X == x && p.Y == y && p.IsWhite == isWhite);
        public bool IsOccupiedByOpponent(int x, int y, bool isWhite) => Pieces.Any(p => p.X == x && p.Y == y && p.IsWhite != isWhite);

        // Path clear excluding endpoints check (useful for sliding pieces)
        public bool IsPathClear(int x1, int y1, int x2, int y2)
        {
            int dx = x2 == x1 ? 0 : (x2 > x1 ? 1 : -1);
            int dy = y2 == y1 ? 0 : (y2 > y1 ? 1 : -1);
            int cx = x1 + dx, cy = y1 + dy;
            while (cx != x2 || cy != y2)
            {
                if (IsOccupied(cx, cy)) return false;
                cx += dx; cy += dy;
            }
            return true;
        }

        public void MovePiece(Puppet piece, int newX, int newY)
        {
            // remove captured
            var target = Pieces.FirstOrDefault(p => p.X == newX && p.Y == newY && p != piece);
            if (target != null) Pieces.Remove(target);

            // move
            piece.X = newX;
            piece.Y = newY;
            piece.HasMoved = true;
        }

        public Table Clone()
        {
            var copy = new Table(false);
            copy.Pieces = this.Pieces.Select(p => p.Clone()).ToList();
            return copy;
        }
    }
}

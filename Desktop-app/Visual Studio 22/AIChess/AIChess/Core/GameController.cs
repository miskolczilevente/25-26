using AIChess.Pieces;
using System;
using System.Linq;

namespace AIChess.Core
{
    public class GameController
    {
        public Table Table { get; private set; }
        private AIPlayer ai;
        public bool IsWhiteTurn { get; private set; } = true;

        public GameController()
        {
            Table = new Table();
            ai = new AIPlayer();
        }

        
        public bool PlayerMove(Puppet piece, int x, int y)
        {
            if (piece == null) return false;
            if (!piece.IsWhite || !IsWhiteTurn) return false;

            var moves = piece.GetMoves(Table);
            if (!moves.Any(m => m.x == x && m.y == y)) return false;

            
            if (piece is King king && Math.Abs(x - king.X) == 2)
            {
                DoCastling(king, x);
            }
            else
            {
                Table.MovePiece(piece, x, y);
            }

            IsWhiteTurn = false;

            
            if (CheckWin(out _)) return true;

            
            AIMove();

            return true;
        }

        private void AIMove()
        {
            var (piece, x, y) = ai.GetBestMove(Table);
            if (piece == null) return;

            
            var realPiece = Table.Pieces.FirstOrDefault(p => p.GetType() == piece.GetType() && p.X == piece.X && p.Y == piece.Y && p.IsWhite == piece.IsWhite);
            if (realPiece == null) return;

            if (realPiece is King && Math.Abs(x - realPiece.X) == 2)
            {
                DoCastling((King)realPiece, x);
            }
            else
            {
                Table.MovePiece(realPiece, x, y);
            }

            IsWhiteTurn = true;
            CheckWin(out _);
        }

        private void DoCastling(King king, int targetX)
        {
            bool kingside = targetX > king.X;
            int rookX = kingside ? 7 : 0;
            var rook = Table.Pieces.OfType<Rook>().FirstOrDefault(r => r.X == rookX && r.Y == king.Y && r.IsWhite == king.IsWhite);
            if (rook == null) return;

            int newKingX = kingside ? 6 : 2;
            int newRookX = kingside ? 5 : 3;

            Table.MovePiece(king, newKingX, king.Y);
            Table.MovePiece(rook, newRookX, rook.Y);
            king.HasMoved = true;
            rook.HasMoved = true;
        }

        
        public bool CheckWin(out bool whiteWon)
        {
            whiteWon = false;
            bool whiteAlive = Table.Pieces.OfType<King>().Any(k => k.IsWhite);
            bool blackAlive = Table.Pieces.OfType<King>().Any(k => !k.IsWhite);

            if (!whiteAlive)
            {
                whiteWon = false;
                return true;
            }
            if (!blackAlive)
            {
                whiteWon = true;
                return true;
            }
            return false;
        }
    }
}

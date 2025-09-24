using AIChess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIChess.Core
{
    public class AIPlayer
    {
        private Random rnd = new Random();

        
        public (Puppet piece, int x, int y) GetBestMove(Table table)
        {
            var moves = new List<(Puppet piece, int x, int y, int score)>();

            foreach (var piece in table.Pieces.Where(p => !p.IsWhite))
            {
                var possible = piece.GetMoves(table);
                foreach (var mv in possible)
                {
                    int x = mv.x, y = mv.y;
                    var tableSim = table.Clone();

                    
                    var pieceSim = tableSim.Pieces.First(p => p.X == piece.X && p.Y == piece.Y && p.GetType() == piece.GetType());

                    int score = 0;
                    
                    var captured = tableSim.Pieces.FirstOrDefault(p => p.X == x && p.Y == y && p.IsWhite != pieceSim.IsWhite);
                    if (captured != null) score += 1;

                    tableSim.MovePiece(pieceSim, x, y);

                    
                    bool willBeCaptured = tableSim.Pieces
                        .Where(p => p.IsWhite)
                        .Any(pw => pw.GetMoves(tableSim).Any(m2 => m2.x == pieceSim.X && m2.y == pieceSim.Y));
                    if (willBeCaptured) score -= 1;

                    moves.Add((piece, x, y, score));
                }
            }

            if (!moves.Any()) return (null, -1, -1);

            int best = moves.Max(m => m.score);
            var bestMoves = moves.Where(m => m.score == best).ToList();
            var chosen = bestMoves[rnd.Next(bestMoves.Count)];
            return (chosen.piece, chosen.x, chosen.y);
        }
    }
}

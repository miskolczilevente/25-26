using AIChess.PuppetsType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIChess
{
    public class AIPlayer
    {
        private Table table;

        public AIPlayer(Table table)
        {
            this.table = table;
        }

        public (Puppet piece, int targetX, int targetY) GetBestMove()
        {
            int bestScore = int.MinValue;
            Puppet bestPiece = null;
            int bestX = -1;
            int bestY = -1;

            // Végigmegy az összes fekete bábun
            foreach (var piece in table.Pieces.Where(p => !p.IsWhite))
            {
                var moves = table.GetMoveFields(piece);

                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if (!moves[x, y].CanMoveHere) continue;

                        // Másold a táblát ideiglenesen
                        var tableCopy = table.Clone();
                        var pieceCopy = tableCopy.Pieces.First(p => p.X == piece.X && p.Y == piece.Y);
                        int score = SimulateMove(pieceCopy, x, y, tableCopy, 2); // 2 lépés mélység

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestPiece = piece;
                            bestX = x;
                            bestY = y;
                        }
                    }
                }
            }

            return (bestPiece, bestX, bestY);
        }

        private int SimulateMove(Puppet piece, int targetX, int targetY, Table tableSim, int depth)
        {
            // Célmezőn lévő bábu levétele
            var target = tableSim.Pieces.FirstOrDefault(p => p.X == targetX && p.Y == targetY);
            if (target != null && target.IsWhite != piece.IsWhite)
                tableSim.Pieces.Remove(target);

            // Mozgatás
            piece.X = targetX;
            piece.Y = targetY;

            // Pontszám kiértékelés
            int score = EvaluateBoard(tableSim);

            if (depth > 1)
            {
                // Ellenfél (fehér) legrosszabb lépései csökkentik a pontszámot
                int minOpponentScore = int.MaxValue;
                foreach (var opp in tableSim.Pieces.Where(p => p.IsWhite))
                {
                    var oppMoves = tableSim.GetMoveFields(opp);
                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            if (!oppMoves[x, y].CanMoveHere) continue;
                            var tableCopy = tableSim.Clone();
                            var oppCopy = tableCopy.Pieces.First(p => p.X == opp.X && p.Y == opp.Y);
                            int s = SimulateMove(oppCopy, x, y, tableCopy, depth - 1);
                            if (s < minOpponentScore) minOpponentScore = s;
                        }
                    }
                }
                score -= (minOpponentScore == int.MaxValue ? 0 : minOpponentScore);
            }

            return score;
        }

        private int EvaluateBoard(Table table)
        {
            int score = 0;
            foreach (var p in table.Pieces)
            {
                int value = p switch
                {
                    Pawn => 1,
                    Knight => 3,
                    Bishop => 3,
                    Rook => 5,
                    Queen => 9,
                    King => 1000,
                    _ => 0
                };

                score += p.IsWhite ? -value : value;
            }
            return score;
        }
    }
}

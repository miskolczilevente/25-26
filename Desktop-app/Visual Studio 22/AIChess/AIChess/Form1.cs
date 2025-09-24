using AIChess.Core;
using AIChess.Pieces;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AIChess
{
    public partial class Form1 : Form
    {
        private const int tileSize = 60;
        private GameController game;
        private Puppet selectedPiece;

        public Form1()
        {
            InitializeComponent(); 
            this.ClientSize = new Size(8 * tileSize, 8 * tileSize);
            this.DoubleBuffered = true;

            game = new GameController();
            this.MouseClick += Form1_MouseClick;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / tileSize;
            int y = e.Y / tileSize;

            if (selectedPiece == null)
            {
                selectedPiece = game.Table.Pieces.FirstOrDefault(p => p.X == x && p.Y == y && p.IsWhite && game.IsWhiteTurn);
            }
            else
            {
                if (game.PlayerMove(selectedPiece, x, y))
                {
                    selectedPiece = null;
                    Invalidate();
                }
                else
                {
                    
                    selectedPiece = game.Table.Pieces.FirstOrDefault(p => p.X == x && p.Y == y && p.IsWhite && game.IsWhiteTurn);
                    Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBoard(e.Graphics);
            DrawMoveFields(e.Graphics);
            DrawPieces(e.Graphics);
        }

        private void DrawBoard(Graphics g)
        {
            for (int yy = 0; yy < 8; yy++)
                for (int xx = 0; xx < 8; xx++)
                {
                    var brush = (xx + yy) % 2 == 0 ? Brushes.Beige : Brushes.Sienna;
                    g.FillRectangle(brush, xx * tileSize, yy * tileSize, tileSize, tileSize);
                }
        }

        private void DrawPieces(Graphics g)
        {
            var font = new Font("Arial", 28, FontStyle.Bold);
            foreach (var piece in game.Table.Pieces)
            {
                Brush brush = piece.IsWhite ? Brushes.White : Brushes.Black;
                float px = piece.X * tileSize + 5;
                float py = piece.Y * tileSize + 5;

                if (piece.IsWhite)
                {
                    
                    g.DrawString(piece.Symbol, font, Brushes.Black, px - 1, py);
                    g.DrawString(piece.Symbol, font, Brushes.Black, px + 1, py);
                    g.DrawString(piece.Symbol, font, Brushes.Black, px, py - 1);
                    g.DrawString(piece.Symbol, font, Brushes.Black, px, py + 1);
                }

                g.DrawString(piece.Symbol, font, brush, px, py);
            }
        }

        private void DrawMoveFields(Graphics g)
        {
            if (selectedPiece == null) return;

            var moves = selectedPiece.GetMoves(game.Table);
            foreach (var mv in moves)
            {
                var brush = new SolidBrush(Color.FromArgb(120, Color.Green));
                g.FillRectangle(brush, mv.x * tileSize, mv.y * tileSize, tileSize, tileSize);
            }
        }
    }
}

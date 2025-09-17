using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AIChess
{
    public partial class Form1 : Form
    {
        private const int tileSize = 60;
        private const int gridSize = 8;
        private Table table;
        private Puppet selectedPiece = null;

        public Form1()
        {
            InitializeComponent();
            table = new Table();
            this.ClientSize = new Size(gridSize * tileSize, gridSize * tileSize);
            this.MouseClick += Form1_MouseClick;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawChessBoard(e.Graphics);
            DrawPieces(e.Graphics);
        }

        private void DrawChessBoard(Graphics g)
        {
            bool white = true;
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    Brush brush = white ? Brushes.Beige : Brushes.Sienna;
                    g.FillRectangle(brush, x * tileSize, y * tileSize, tileSize, tileSize);

                    if (selectedPiece != null && selectedPiece.X == x && selectedPiece.Y == y)
                        g.DrawRectangle(new Pen(Color.Red, 3), x * tileSize, y * tileSize, tileSize, tileSize);

                    white = !white;
                }
                white = !white;
            }
        }

        private void DrawPieces(Graphics g)
        {
            var font = new Font("Arial", 28, FontStyle.Bold);
            foreach (var piece in table.Pieces)
            {
                float posX = piece.X * tileSize + 5;
                float posY = piece.Y * tileSize + 5;

                if (piece.IsWhite)
                {
                    g.DrawString(piece.Symbol, font, Brushes.Black, posX - 1, posY);
                    g.DrawString(piece.Symbol, font, Brushes.Black, posX + 1, posY);
                    g.DrawString(piece.Symbol, font, Brushes.Black, posX, posY - 1);
                    g.DrawString(piece.Symbol, font, Brushes.Black, posX, posY + 1);
                    g.DrawString(piece.Symbol, font, Brushes.White, posX, posY);
                }
                else
                {
                    g.DrawString(piece.Symbol, font, Brushes.Black, posX, posY);
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / tileSize;
            int y = e.Y / tileSize;

            if (selectedPiece == null)
            {
                selectedPiece = table.Pieces.FirstOrDefault(p => p.X == x && p.Y == y);
            }
            else
            {
                if (selectedPiece.IsValidMove(x, y, table))
                {
                    var target = table.Pieces.FirstOrDefault(p => p.X == x && p.Y == y);
                    if (target != null && target.IsWhite != selectedPiece.IsWhite)
                        table.Pieces.Remove(target);

                    selectedPiece.X = x;
                    selectedPiece.Y = y;
                }

                selectedPiece = null;
            }

            Invalidate();
        }
    }
}

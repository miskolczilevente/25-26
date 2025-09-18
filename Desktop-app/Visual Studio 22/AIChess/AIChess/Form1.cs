using AIChess.PuppetsType;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace AIChess
{
    public partial class Form1 : Form
    {
        private const int tileSize = 60;
        private Table table;
        private GameController controller;
        private Puppet selectedPiece;
        private Field[,] moveFields = new Field[8, 8];

        public Form1()
        {
            InitializeComponent();
            this.ClientSize = new Size(tileSize * 8, tileSize * 8);

            table = new Table();
            controller = new GameController(table);

            this.MouseClick += Form1_MouseClick;
            InitializeMoveFields();
        }

        private void InitializeMoveFields()
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    moveFields[x, y] = new Field(x, y);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawChessBoard(e.Graphics);
            DrawMoveFields(e.Graphics);
            DrawPieces(e.Graphics);
        }

        private void DrawChessBoard(Graphics g)
        {
            bool white = true;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Brush brush = white ? Brushes.Beige : Brushes.Sienna;
                    g.FillRectangle(brush, x * tileSize, y * tileSize, tileSize, tileSize);
                    white = !white;
                }
                white = !white;
            }
        }

        private void DrawPieces(Graphics g)
        {
            foreach (var piece in table.Pieces)
            {
                var font = new Font("Arial", 28, FontStyle.Bold);
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

        private void DrawMoveFields(Graphics g)
        {
            if (selectedPiece == null) return;

            var fields = controller.GetMoveFields(selectedPiece);
            foreach (var f in fields)
            {
                Brush brush = new SolidBrush(Color.FromArgb(128, Color.Green));
                g.FillRectangle(brush, f.X * tileSize, f.Y * tileSize, tileSize, tileSize);
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / tileSize;
            int y = e.Y / tileSize;

            if (selectedPiece == null)
            {
                // Kijelölés
                selectedPiece = table.Pieces.FirstOrDefault(p => p.X == x && p.Y == y && p.IsWhite == controller.IsWhiteTurn);
            }
            else
            {
                // Mozgatás próbálkozás
                if (controller.TryMove(selectedPiece, x, y, out string winner))
                {
                    if (winner != null)
                    {
                        MessageBox.Show($"{winner} nyert!");
                        this.Close();
                        return;
                    }
                }
                selectedPiece = null;
            }

            Invalidate();
        }
    }
}
